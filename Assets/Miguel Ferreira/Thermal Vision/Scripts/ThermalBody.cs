using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ThermalBody : MonoBehaviour {

	[Range(0.5f,8f)]
	public float ThermalDistribution = 4f;
	[Range(0f,1f)]
	public float MaximumTemperature = 1f;
	[Range(0f,1f)]
	public float MinimumTemperature = 0f;


	private readonly List<Renderer> _childrenRenderers = new List<Renderer>(8);

	private void Awake()
	{
		GetComponentsInChildren<Renderer> (_childrenRenderers);
	}

	void OnEnable()
	{
		UpdateSubMaterialProperties();
	}

	void UpdateSubMaterialProperties()
	{
		foreach (var childrenRenderer in _childrenRenderers) {
			var materials = childrenRenderer.sharedMaterials;
			foreach (var material in materials) {
				if (MaximumTemperature > 0.3f) {
					material.SetOverrideTag ("Thermal", "Hot");
				} else {
					material.SetOverrideTag ("Thermal", "Cold");
				}

				material.SetFloat ("_ThermalPowExponent", ThermalDistribution);
				material.SetFloat ("_ThermalMax", MaximumTemperature);
				material.SetFloat ("_ThermalMin", MinimumTemperature);
			}
		}
	}

	void OnDisable() {
		foreach (var childrenRenderer in _childrenRenderers) {
			var materials = childrenRenderer.sharedMaterials;
			foreach (var material in materials) {
				material.SetOverrideTag ("Thermal", "");
			}
		}
	}
	
#if UNITY_EDITOR
	private void Update()
	{
		//Enable quick changes in the editor
		UpdateSubMaterialProperties();
	}
#endif
}
