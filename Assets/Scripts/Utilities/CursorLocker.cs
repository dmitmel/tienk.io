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

namespace Tienkio.Utilities {
    public class CursorLocker : Singleton<CursorLocker> {
        public bool cursorIsLocked = true;

        void Start() {
            if (cursorIsLocked) LockCursor();
        }

        public void LockCursor() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorIsLocked = true;
        }

        public void UnlockCursor() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorIsLocked = false;
        }

        void Update() {
            if (Input.GetButtonDown("Toggle Cursor")) {
                if (cursorIsLocked) UnlockCursor();
                else LockCursor();
            }
        }
    }
}
