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
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tienkio {
    public class NickFieldController : MonoBehaviour {
        public int gameSceneIndex;

        InputField inputField;

        void Awake() {
            inputField = GetComponent<InputField>();
        }

        public void OnSubmit(BaseEventData eventData) {
            Debug.Log(eventData);
        }

        public void StartGame(string nick) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                PlayerPrefs.SetString("nick", nick);
                SceneManager.LoadScene(gameSceneIndex);
            }
        }
    }
}
