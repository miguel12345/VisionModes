using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MiguelFerreira {

	public class VisionModeSwticher : MonoBehaviour {

		[Serializable]
		public struct VisionModeTrigger {
			public KeyCode Key;
			public MonoBehaviour VisionMode;
		}

		public VisionModeTrigger[] VisionModeTriggers;
		MonoBehaviour currentVisionMode;

		void Update() {
			var numVisionModes = VisionModeTriggers.Length;

			for (int i = 0; i < numVisionModes; i++) {
				if (Input.GetKeyDown (VisionModeTriggers [i].Key)) {

					var newVisionMode = VisionModeTriggers [i].VisionMode;

					if (currentVisionMode != newVisionMode && currentVisionMode!= null) {
						currentVisionMode.enabled = false;
					}

					currentVisionMode = VisionModeTriggers [i].VisionMode;
					currentVisionMode.enabled = !currentVisionMode.enabled;
				}
			}
		}
	}
}