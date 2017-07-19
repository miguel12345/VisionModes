using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

namespace MiguelFerreira
{
	[ExecuteInEditMode]
	public class ElectroMagneticBody : MonoBehaviour {

		[Range(0f,1f)]
		public float ElectroMagneticStrength = 1f;
		
		public Renderer[] Renderers
		{
			get;
			private set;
		}

		public Material EMMaterial{
			get;
			private set;
		}
		
		public int[] SubMeshCounts {
			get;
			private set;
		}

		private void Awake()
		{
			Renderers = GetComponentsInChildren<Renderer>();
			
			var subMeshCountList = new List<int>();

			foreach (var subRenderer in Renderers)
			{
				if (subRenderer is SkinnedMeshRenderer)
				{
					subMeshCountList.Add(((SkinnedMeshRenderer) subRenderer).sharedMesh.subMeshCount);
				}
				else
				{
					subMeshCountList.Add(subRenderer.GetComponent<MeshFilter>().sharedMesh.subMeshCount);
				}
			}
			
			SubMeshCounts = subMeshCountList.ToArray();
			
			EMMaterial = new Material(Shader.Find("Hidden/EMVision"));
			EMMaterial.SetFloat("_EMStrength",ElectroMagneticStrength);
		}
			
	}

}

