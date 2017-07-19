using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiguelFerreira {

	[ExecuteInEditMode]
	public class NightVisionMode : MonoBehaviour {

		private Material material;
		[Range(0f,1f)]
		public float Strength = 0.2f;

		private RenderingPath ownCameraOriginalRenderingPath;

		void Awake ()
		{
			material = new Material( Shader.Find("MiguelFerreira/NightVision") );
		}

		private void OnEnable()
		{
			var ownCamera = GetComponent<Camera>();
			ownCameraOriginalRenderingPath = ownCamera.renderingPath;
			ownCamera.renderingPath = RenderingPath.DeferredShading;
		}

		private void OnDisable()
		{
			var ownCamera = GetComponent<Camera>();
			ownCamera.renderingPath = ownCameraOriginalRenderingPath;
		}

		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			material.SetFloat ("_DeferredContribution", Strength);
			Graphics.Blit (source, destination, material);
		}
	}
}