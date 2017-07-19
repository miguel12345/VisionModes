using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering;

namespace MiguelFerreira
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class EMVisionMode : MonoBehaviour
	{
		
		[Range(0f,100f)]
		public float MagneticEffectSpeed = 20;
		[Range(0f,200f)]
		public float MagneticEffectFrequency = 120;
		[Range(0f,1f)]
		public float MagneticEffectStrength = 0.1f;
		
		private Camera ownCamera;
		private RenderingPath ownCameraOriginalRenderingPath;
		private bool _wasPostProcessingBehaviourEnabled;
		private ElectroMagneticBody[] _emBodies;
		private CommandBuffer _commandBuffer;
		private int _prePassEMBodiesRenderTexID;
		private int _blurPassRenderTexID;
		private int _tempRenderTexID;
		private int _blurSizeID;
		private Vector2 _blurTexelSize;
		private Material _blurMaterial;
		private Material _finalCompositeMaterial;
		private int _magneticEffectSpeedPropertyId;
		private int _magneticEffectFrequencyPropertyId;
		private int _magneticEffectStrengthPropertyId;

		private void Awake()
		{
			_blurMaterial = new Material(Shader.Find("Hidden/Blur"));
			
			_prePassEMBodiesRenderTexID = Shader.PropertyToID("_EMPrePassTex");
			_blurPassRenderTexID = Shader.PropertyToID("_EMBlurredTex");
			_tempRenderTexID = Shader.PropertyToID("_TempTex0");
			_blurSizeID = Shader.PropertyToID("_BlurSize");
			_finalCompositeMaterial = new Material(Shader.Find("Hidden/EMCompositeShader"));
			_magneticEffectSpeedPropertyId = Shader.PropertyToID("_MagneticEffectSpeed");
			_magneticEffectFrequencyPropertyId = Shader.PropertyToID("_MagneticEffectFrequency");
			_magneticEffectStrengthPropertyId = Shader.PropertyToID("_MagneticEffectStrength");
		}

		private void OnEnable()
		{
			ownCamera = GetComponent<Camera>();
			var postProcessingBehaviour = ownCamera.GetComponent<PostProcessingBehaviour>();
			_wasPostProcessingBehaviourEnabled = postProcessingBehaviour != null && postProcessingBehaviour.enabled;
			ownCameraOriginalRenderingPath = ownCamera.renderingPath;
			ownCamera.renderingPath = RenderingPath.Forward; 
			ownCamera.SetReplacementShader(Shader.Find("Hidden/EMVision"),"RenderType");
			
			//TODO We should design a better way to register/unregister EM bodies, instead of having to invoke this (slow) method.
			_emBodies = FindObjectsOfType<ElectroMagneticBody>();
			_commandBuffer = new CommandBuffer {name = "EM Bodies"};

			ownCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
			RebuildCommandBuffer();
		}

		private void OnDisable()
		{
			if (_wasPostProcessingBehaviourEnabled)
			{
				GetComponent<PostProcessingBehaviour>().enabled = true;
			}
			ownCamera.renderingPath = ownCameraOriginalRenderingPath;
			ownCamera.ResetReplacementShader();
			ownCamera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
		}

		private void RebuildCommandBuffer()
		{
			var renderTargetWidth = Screen.width;
			var renderTargetHeight = Screen.height;
			
			var targetTexture = ownCamera.targetTexture;

			if (targetTexture != null)
			{
				renderTargetWidth = targetTexture.width;
				renderTargetHeight = targetTexture.height;
			}
			
			_commandBuffer.Clear();
			
			
			_commandBuffer.GetTemporaryRT(_prePassEMBodiesRenderTexID, renderTargetWidth, renderTargetHeight, 0, FilterMode.Bilinear, RenderTextureFormat.RFloat, RenderTextureReadWrite.Default, QualitySettings.antiAliasing);
			_commandBuffer.SetRenderTarget(_prePassEMBodiesRenderTexID);
			_commandBuffer.ClearRenderTarget(true, true, Color.clear);

			for (var i = 0; i < _emBodies.Length; i++)
			{
				var emBody = _emBodies[i];

				for (var a = 0; a < emBody.Renderers.Length; a++)
				{
					var submeshCount = emBody.SubMeshCounts[a];

					for (var submeshIndex = 0; submeshIndex < submeshCount; submeshIndex++)
					{
						_commandBuffer.DrawRenderer(emBody.Renderers[a],emBody.EMMaterial,submeshIndex);
					}

				}
			}
			
			_commandBuffer.GetTemporaryRT(_blurPassRenderTexID , renderTargetWidth >> 1, renderTargetHeight >> 1, 0, FilterMode.Bilinear, RenderTextureFormat.RFloat);
			_commandBuffer.GetTemporaryRT(_tempRenderTexID, renderTargetWidth >> 1, renderTargetHeight >> 1, 0, FilterMode.Bilinear, RenderTextureFormat.RFloat);
			_commandBuffer.Blit(_prePassEMBodiesRenderTexID, _blurPassRenderTexID);

			_blurTexelSize = new Vector2(1f / (renderTargetWidth >> 1), 1f / (renderTargetHeight >> 1));
			_commandBuffer.SetGlobalVector(_blurSizeID, _blurTexelSize);

			for (var i = 0; i < 4; i++)
			{
				_commandBuffer.Blit(_blurPassRenderTexID, _tempRenderTexID, _blurMaterial, 0);
				_commandBuffer.Blit(_tempRenderTexID, _blurPassRenderTexID, _blurMaterial, 1);
			}
			
			_commandBuffer.ReleaseTemporaryRT(_tempRenderTexID);
			_commandBuffer.ReleaseTemporaryRT(_prePassEMBodiesRenderTexID);
			
			_finalCompositeMaterial.SetFloat(_magneticEffectSpeedPropertyId,MagneticEffectSpeed);
			_finalCompositeMaterial.SetFloat(_magneticEffectFrequencyPropertyId,MagneticEffectFrequency);
			_finalCompositeMaterial.SetFloat(_magneticEffectStrengthPropertyId,MagneticEffectStrength);
		}

		private void Update()
		{
			RebuildCommandBuffer();
		}

		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			Graphics.Blit(src,dest,_finalCompositeMaterial);
		}
	}


}

