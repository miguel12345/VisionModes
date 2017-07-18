using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiguelFerreira {

	[ExecuteInEditMode]
	public class ThermalVisionMode : MonoBehaviour {
		
		public Shader thermalShader;
		public Texture2D thermalLookUpTexture;
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
			MainCamera.SetReplacementShader (thermalShader, "Thermal");
		}

		void OnDisable() {
			
			MainCamera.renderingPath = mainCameraOriginalRenderingPath;
			MainCamera.ResetReplacementShader ();
		}
	}
}