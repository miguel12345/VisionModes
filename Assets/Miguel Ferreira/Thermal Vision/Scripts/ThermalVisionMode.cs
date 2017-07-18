using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiguelFerreira {

	[ExecuteInEditMode]
	public class ThermalVisionMode : MonoBehaviour {
		
		public Shader thermalShader;
		public Texture2D thermalLookUpTexture;
		RenderingPath mainCameraOriginalRenderingPath;
		bool mainCameraOriginalOcclusionCulling;

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

			mainCameraOriginalOcclusionCulling = MainCamera.useOcclusionCulling;
			mainCameraOriginalRenderingPath = MainCamera.renderingPath;
			MainCamera.useOcclusionCulling = false;
			MainCamera.renderingPath = RenderingPath.Forward;
			MainCamera.SetReplacementShader (thermalShader, "Thermal");
		}

		void OnDisable() {

			MainCamera.useOcclusionCulling = mainCameraOriginalOcclusionCulling;
			MainCamera.renderingPath = mainCameraOriginalRenderingPath;
			MainCamera.ResetReplacementShader ();
		}
	}
}