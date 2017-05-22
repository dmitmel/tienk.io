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

namespace Tienkio {
    public class Settings : Singleton<Settings> {
        public SwitchButton controlsType, inversedControls;
        public Slider sensetivity, musicVolume, effectsVolume;

        void Start() {
            controlsType.value = PlayerPrefs.GetInt("controlsType", 0);
            inversedControls.value = PlayerPrefs.GetInt("inversedControls", 0);
            sensetivity.value = PlayerPrefs.GetFloat("sensetivity", 0.5f);
            musicVolume.value = PlayerPrefs.GetFloat("musicVolume", 1);
            effectsVolume.value = PlayerPrefs.GetFloat("effectsVolume", 1);
        }

        public void SaveControlsType(int controlsTypeIndex) {
            PlayerPrefs.SetInt("controlsType", controlsTypeIndex);
        }

        public void SaveInversedControls(int invertControlsIndex) {
            PlayerPrefs.SetInt("inversedControls", invertControlsIndex);
        }

        public void SaveSensetivity(float sensetivity) {
            PlayerPrefs.SetFloat("sensetivity", sensetivity);
        }

        public void SaveMusicVolume(float musicVolume) {
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
        }

        public void SaveEffectsVolume(float effectsVolume) {
            PlayerPrefs.SetFloat("effectsVolume", effectsVolume);
        }
    }
}
