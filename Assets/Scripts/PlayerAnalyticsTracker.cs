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
using UnityEngine.Analytics;
using System.Collections.Generic;

namespace Tienkio {
    public class PlayerAnalyticsTracker : MonoBehaviour {
        public TankController tank;

        static int maxReachedLevel;

        public void OnGameOver() {
            Analytics.CustomEvent("GameOver", new Dictionary<string, object> {
                { "kills", tank.kills }
            });
        }

        public void OnScoreChange() {
            int currentLevel = tank.scoreCounter.currentLevel.index;
            for (int level = maxReachedLevel + 1; level <= currentLevel; level++) {
                Analytics.CustomEvent("ReachNewLevel", new Dictionary<string, object> {
                    { "level", level }
                });
                maxReachedLevel = level;
            }
        }
    }
}
