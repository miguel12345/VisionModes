using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiguelFerreira {

	[ExecuteInEditMode]
	public class NightVisionMode : MonoBehaviour {

		private Material material;
		[Range(0f,1f)]
		public float Strength = 0.2f;

		void Awake ()
		{
			material = new Material( Shader.Find("MiguelFerreira/NightVision") );
		}

		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			material.SetFloat ("_DeferredContribution", Strength);
			Graphics.Blit (source, destination, material);
		}
	}
}