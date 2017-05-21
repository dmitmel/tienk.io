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

namespace Tienkio {
    public class Modal : MonoBehaviour {
        public static int openedPausedModalsCount { get; private set; }

        public GameObject modalBox;
        public bool pauseGame, unlockCursor;
        public bool isOpened { get; private set; }

        void Update() {
            Time.timeScale = openedPausedModalsCount > 0 ? 0 : 1;
        }

        public void ShowModal() {
            if (!isOpened) {
                if (pauseGame) openedPausedModalsCount++;
                if (unlockCursor && CursorLocker.instanceExists) CursorLocker.instance.UnlockCursor();
                modalBox.SetActive(true);
                isOpened = true;
            }
        }

        public void CloseModal() {
            if (isOpened) {
                if (pauseGame) openedPausedModalsCount--;
                if (unlockCursor && CursorLocker.instanceExists) CursorLocker.instance.LockCursor();
                modalBox.SetActive(false);
                isOpened = false;
            }
        }
    }
}
