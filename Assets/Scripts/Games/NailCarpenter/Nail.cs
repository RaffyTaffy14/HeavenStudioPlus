using UnityEngine;

using HeavenStudio.Util;
using HeavenStudio.InputSystem;

namespace HeavenStudio.Games.Scripts_NailCarpenter
{
    public class Nail : MonoBehaviour
    {
        public double targetBeat;
        public float targetX;
        public float metresPerSecond;
        public Animator nailAnim;

        private NailCarpenter game;

        public void Init()
        {
            game = NailCarpenter.instance;

            game.ScheduleInput(targetBeat, 0, NailCarpenter.InputAction_RegPress, HammmerJust, HammmerMiss, null);
            //wrong input
            if (PlayerInput.CurrentControlStyle != InputController.ControlStyles.Touch)
            {
                game.ScheduleUserInput(targetBeat, 0, NailCarpenter.InputAction_AltPress, StrongHammmerJust, null, null);
            }
            targetBeat = Conductor.instance.GetUnSwungBeat(targetBeat);
            Update();
        }

        private void HammmerJust(PlayerActionEvent caller, float state)
        {
            game.Carpenter.DoScaledAnimationAsync("carpenterHit", 0.25f);
            if (state >= 1f || state <= -1f)
            {
                nailAnim.DoScaledAnimationAsync(
                    (state >= 1f ? "nailBendRight" : "nailBendLeft"), 0.25f);
                SoundByte.PlayOneShot("miss");
                return;
            }
            SoundByte.PlayOneShotGame("nailCarpenter/HammerWeak");
            nailAnim.DoScaledAnimationAsync("nailHammered", 0.25f);
        }
        private void StrongHammmerJust(PlayerActionEvent caller, float state)
        {
            game.ScoreMiss();
            game.Carpenter.DoScaledAnimationAsync("carpenterHit", 0.25f);
            if (state >= 1f || state <= -1f)
            {
                nailAnim.DoScaledAnimationAsync(
                    (state >= 1f ? "nailBendRight" : "nailBendLeft"), 0.25f);
                SoundByte.PlayOneShot("miss");
                return;
            }
            SoundByte.PlayOneShotGame("nailCarpenter/HammerStrong");
            nailAnim.DoScaledAnimationAsync("nailStrongHammered", 0.25f);
        }

        private void HammmerMiss(PlayerActionEvent caller)
        {
            game.Carpenter.DoScaledAnimationAsync("eyeBlink", 0.25f, animLayer: 1);
            nailAnim.DoScaledAnimationAsync("nailMiss", 0.5f);
        }

        private void Update()
        {
            var cond = Conductor.instance;

            if (cond.isPlaying && !cond.isPaused)
            {
                double beat = cond.unswungSongPositionInBeatsAsDouble;
                Vector3 pos = transform.position;
                pos.x = targetX + (float)((beat - targetBeat) * metresPerSecond);
                transform.position = pos;
                if (targetBeat != double.MinValue)
                {
                    if (beat >= targetBeat + 9) Destroy(gameObject);
                }
            }
        }
    }
}