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
    public class BouncyObject : MonoBehaviour {
        public int knockback;

        new Transform transform;
        new Rigidbody2D rigidbody;

        void Awake() {
            transform = base.transform;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            Rigidbody2D colliderRigidbody = collision.collider.attachedRigidbody;
            if (colliderRigidbody != null && rigidbody != null) {
                Vector2 collisionDirection = (transform.position - collision.transform.position).normalized;

                colliderRigidbody.AddForce(collisionDirection * -knockback, ForceMode2D.Impulse);
                rigidbody.AddForce(collisionDirection * knockback, ForceMode2D.Impulse);
            }
        }
    }
}
