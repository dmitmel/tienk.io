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
using Tienkio.Pools;

namespace Tienkio.Tanks {
    public class TankBullets : MonoBehaviour {
        TankController tankController;
        PoolManager bulletPool;

        int prevRequiredBullets;

        void Awake() {
            tankController = GetComponent<TankController>();
            bulletPool = BulletPool.poolManager;
        }

        void Start() {
            AllocRequiredBullets();
        }

        public void AllocRequiredBullets() {
            int requiredBullets = GetRequiredBulletsCount();
            bulletPool.Free(prevRequiredBullets);
            bulletPool.Alloc(requiredBullets);
            prevRequiredBullets = requiredBullets;
        }

        int GetRequiredBulletsCount() {
            int requiredBullets = 1;
            foreach (Gun gun in tankController.guns)
                requiredBullets += gun.requiredBullets;
            return requiredBullets;
        }
    }
}
