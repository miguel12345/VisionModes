using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiguelFerreira {

	[ExecuteInEditMode]
	public class ThermalVisionMode : MonoBehaviour {
		
		public Shader thermalShader;
		public Texture2D thermalLookUpTexture;

		[Range(0.5f,8f)]
		public float EnvironmentThermalDistribution = 1f;
		[Range(0f,1f)]
		public float EnvironmentMaximumTemperature = 0.1f;

		[Range(0.5f,8f)]
		public float HumanThermalDistribution = 4f;
		[Range(0f,1f)]
		public float HumanMaximumTemperature = 1f;

		[Range(0.5f,8f)]
		public float AlienThermalDistribution = 4f;
		[Range(0f,1f)]
		public float AlienMaximumTemperature = 1f;

		RenderingPath mainCameraOriginalRenderingPath;

		Camera _mainCamera;
		Camera MainCamera {
			get { 
				if (_mainCamera == null) {
					_mainCamera = GetComponent<Camera> ();
				}

				return _mainCamera;
			}
		}

		void OnEnable() {

			Shader.SetGlobalTexture ("_ThermalColorLUT",thermalLookUpTexture);

			mainCameraOriginalRenderingPath = MainCamera.renderingPath;
			MainCamera.renderingPath = RenderingPath.Forward;
			MainCamera.SetReplacementShader (thermalShader, "ThermalType");

		}

		void OnDisable() {
			
			MainCamera.renderingPath = mainCameraOriginalRenderingPath;
			MainCamera.ResetReplacementShader ();
		}

		void OnPreRender() {
			UpdateShaderGlobals ();
		}

		void UpdateShaderGlobals() {
			Shader.SetGlobalFloat ("_ThermalPowExponentEnvironment",EnvironmentThermalDistribution);
			Shader.SetGlobalFloat ("_ThermalMaxEnvironment",EnvironmentMaximumTemperature);

			Shader.SetGlobalFloat ("_ThermalPowExponentHuman",HumanThermalDistribution);
			Shader.SetGlobalFloat ("_ThermalMaxHuman",HumanMaximumTemperature);

			Shader.SetGlobalFloat ("_ThermalPowExponentAlien",AlienThermalDistribution);
			Shader.SetGlobalFloat ("_ThermalMaxAlien",AlienMaximumTemperature);
		}
	}
}