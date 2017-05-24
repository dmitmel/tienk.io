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
using Tienkio.Utilities;

namespace Tienkio.UI {
    public class Modal : MonoBehaviour {
        public bool pauseGame, unlockCursor;
        public bool isOpened { get; private set; }
        bool isCloseable;

        static int openedPauseModals;

        void Update() {
            if (isOpened && !isCloseable) {
                isCloseable = true;
                if (ModalManager.instanceExists) ModalManager.instance.closableModals.Add(this);
            }
        }

        public void ShowModal() {
            if (isOpened) return;

            if (pauseGame) {
                if (openedPauseModals <= 0) {
                    openedPauseModals = 0;
                    Time.timeScale = 0;
                }

                openedPauseModals++;
            }

            if (unlockCursor && CursorLocker.instanceExists) CursorLocker.instance.UnlockCursor();
            gameObject.SetActive(true);

            isOpened = true;
        }

        public void CloseModal() {
            if (!isOpened || !isCloseable) return;

            if (pauseGame) {
                openedPauseModals--;

                if (openedPauseModals <= 0) {
                    openedPauseModals = 0;
                    Time.timeScale = 1;
                }
            }

            if (unlockCursor && CursorLocker.instanceExists) CursorLocker.instance.LockCursor();
            gameObject.SetActive(false);

            if (ModalManager.instanceExists) ModalManager.instance.closableModals.Remove(this);
            isCloseable = isOpened = false;
        }

        void OnDestroy() {
            CloseModal();
        }
    }
}
