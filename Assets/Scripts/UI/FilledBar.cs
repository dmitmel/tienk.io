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

namespace Tienkio.UI {
    public class FilledBar : Bar {
        [SerializeField, Space]
        protected Image _bar;

        public Image bar {
            get { return _bar; }
            set {
                if (_bar != value) {
                    _bar = value;
                    Resize();
                }
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate() {
            _bar.type = Image.Type.Filled;
            switch (_direction) {
                case Direction.LeftToRight:
                    _bar.fillMethod = Image.FillMethod.Horizontal;
                    _bar.fillOrigin = (int) Image.OriginHorizontal.Left;
                    break;
                case Direction.RightToLeft:
                    _bar.fillMethod = Image.FillMethod.Horizontal;
                    _bar.fillOrigin = (int) Image.OriginHorizontal.Right;
                    break;
                case Direction.BottomToTop:
                    _bar.fillMethod = Image.FillMethod.Vertical;
                    _bar.fillOrigin = (int) Image.OriginVertical.Bottom;
                    break;
                case Direction.TopToBottom:
                    _bar.fillMethod = Image.FillMethod.Vertical;
                    _bar.fillOrigin = (int) Image.OriginVertical.Top;
                    break;
                case Direction.BackToForward:
                    Debug.LogError("FilledBar - direction 'Back To Forward' is not supported by FilledBar");
                    break;
                case Direction.ForwardToBack:
                    Debug.LogError("FilledBar - direction 'Forward To Back' is not supported by FilledBar");
                    break;
            }

            base.OnValidate();
        }
#endif

        public override void Resize() {
            _bar.fillAmount = fill;
        }
    }
}
