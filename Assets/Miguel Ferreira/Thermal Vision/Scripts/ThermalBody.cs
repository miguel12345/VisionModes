using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalBody : MonoBehaviour {

	List<Renderer> childrenRenderers = new List<Renderer>(8);

	protected virtual string GetThermalTypeName() {return "";}

	void OnEnable() {
		GetComponentsInChildren<Renderer> (childrenRenderers);

		foreach (var childrenRenderer in childrenRenderers) {
			var materials = childrenRenderer.sharedMaterials;
			foreach (var material in materials) {
				material.SetOverrideTag ("ThermalType", GetThermalTypeName());
			}
		}
	}

	void OnDisable() {
		foreach (var childrenRenderer in childrenRenderers) {
			var materials = childrenRenderer.sharedMaterials;
			foreach (var material in materials) {
				material.SetOverrideTag ("ThermalType", "");
			}
		}
	}
}
