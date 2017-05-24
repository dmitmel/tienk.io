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
using Tienkio.Tanks;

namespace Tienkio.UI {
    public class LevelBar : MonoBehaviour {
        public int widthOffset;
        public ScoreCounter scoreCounter;

        RectTransform rectTransform;
        Vector2 originalPosition;
        Vector2 originalScale;

        void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        void Start() {
            originalPosition = rectTransform.anchoredPosition;
            originalScale = rectTransform.sizeDelta;
        }

        public void UpdateBar() {
            int score = scoreCounter.score;
            Level currentLevel = scoreCounter.currentLevel;

            if (currentLevel.scoreToNextLevel > 0) {
                int scoreInLevel = score - currentLevel.neededScore;
                float width = widthOffset + scoreInLevel * ((originalScale.x - widthOffset) / currentLevel.scoreToNextLevel);
                rectTransform.sizeDelta = new Vector2(width, originalScale.y);
                float x = width / 2f - originalScale.x / 2f;
                rectTransform.anchoredPosition = new Vector2(x, originalPosition.y);
            } else {
                rectTransform.sizeDelta = originalScale;
                rectTransform.anchoredPosition = originalPosition;
            }
        }
    }
}
