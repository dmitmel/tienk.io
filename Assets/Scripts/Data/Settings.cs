//
//  Copyright (c) 2017  FederationOfCoders.org
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using UnityEngine;
using UnityEngine.UI;
using Tienkio.Utilities;
using Tienkio.UI;

namespace Tienkio.Data {
    public enum ControlsType { WASDMovement, WASDTilt }

    public class Settings : Singleton<Settings> {
        public static ControlsType controlsTypeValue = ControlsType.WASDMovement;
        public static bool inversedControlsValue;
        public static float sensetivityValue = 10, musicVolumeValue = 1, effectsVolumeValue = 1;

        public SwitchButton controlsType, inversedControls;
        public Slider sensitivity, musicVolume, effectsVolume;

        protected override void Awake() {
            base.Awake();

            controlsType.value = PlayerPrefs.GetInt("controlsType", 0);
            if (controlsType.value == 1) controlsTypeValue = ControlsType.WASDTilt;
            else controlsTypeValue = ControlsType.WASDMovement;

            inversedControls.value = PlayerPrefs.GetInt("inversedControls", 0);
            inversedControlsValue = (inversedControls.value == 1);

            sensetivityValue = sensitivity.value = PlayerPrefs.GetFloat("sensitivity", 10);
            musicVolumeValue = musicVolume.value = PlayerPrefs.GetFloat("musicVolume", 1);
            effectsVolumeValue = effectsVolume.value = PlayerPrefs.GetFloat("effectsVolume", 1);
        }

        public void SaveControlsType(int controlsTypeIndex) {
            PlayerPrefs.SetInt("controlsType", controlsTypeIndex);
            if (controlsType.value == 1) controlsTypeValue = ControlsType.WASDTilt;
            else controlsTypeValue = ControlsType.WASDMovement;
        }

        public void SaveInversedControls(int invertControlsIndex) {
            PlayerPrefs.SetInt("inversedControls", invertControlsIndex);
            inversedControlsValue = (inversedControls.value == 1);
        }

        public void SaveSensetivity(float sensitivity) {
            PlayerPrefs.SetFloat("sensitivity", sensitivity);
            sensetivityValue = sensitivity;
        }

        public void SaveMusicVolume(float musicVolume) {
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            musicVolumeValue = musicVolume;
        }

        public void SaveEffectsVolume(float effectsVolume) {
            PlayerPrefs.SetFloat("effectsVolume", effectsVolume);
            effectsVolumeValue = effectsVolume;
        }
    }
}
