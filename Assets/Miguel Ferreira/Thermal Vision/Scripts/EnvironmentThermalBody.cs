using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentThermalBody : ThermalBody {
	
	protected override string GetThermalTypeName() {return "Environment";}

}
