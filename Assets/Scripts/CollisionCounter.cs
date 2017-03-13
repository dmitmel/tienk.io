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

namespace Deepio {
    public class CollisionCounter : MonoBehaviour {
        int collisions;

        void OnTriggerEnter2D(Collider2D collider) {
            collisions = 0;
        }

        void OnTriggerStay2D(Collider2D collider) {
            collisions++;
        }

        void OnTriggerExit2D(Collider2D collider) {
            Debug.Log(collisions, gameObject);
            collisions = 0;
        }

        void OnCollisionEnter2D(Collision2D collision) {
            collisions = 0;
        }

        void OnCollisionStay2D(Collision2D collision) {
            collisions++;
        }

        void OnCollisionExit2D(Collision2D collision) {
            Debug.Log(collisions, gameObject);
            collisions = 0;
        }
    }
}
