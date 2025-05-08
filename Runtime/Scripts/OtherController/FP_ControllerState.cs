namespace FuzzPhyte.Control
{
    using UnityEngine;
    public interface IFPControllerState 
    {
        void OnEnter(FP_CharacterController controller);
        void UpdateState(FP_CharacterController controller);
        void OnExit(FP_CharacterController controller);
    }
    public class FP_IdleState : IFPControllerState
    {
        public void OnEnter(FP_CharacterController controller) { /* Logic for entering idle */ }
        public void UpdateState(FP_CharacterController controller) { /* Check for transitions */ }
        public void OnExit(FP_CharacterController controller) { /* Cleanup or animation changes */ }
    }
    public class FP_WalkState : IFPControllerState
    {
        public void OnEnter(FP_CharacterController controller)
        {
            // Set walk animation trigger here if needed
        }

        public void UpdateState(FP_CharacterController controller)
        {
            if (controller.MovementInput.magnitude > 0.01f)
            {
                controller.ApplyMovement(controller.MovementInput);
            }
            else
            {
                controller.SetState(new FP_IdleState());
                return;
            }

            if (controller.JumpInput)
            {
                controller.SetState(new FP_JumpState());
                return;
            }

            if (controller.CrouchInput)
            {
                controller.SetState(new FP_CrouchState());
                return;
            }

            if (controller.SlideInput)
            {
                controller.SetState(new FP_SlideState());
                return;
            }
        }

        public void OnExit(FP_CharacterController controller)
        {
            // Cleanup walk state (e.g., animation flags)
        }
    }

    public class FP_JumpState : IFPControllerState
    {
        private float elapsed = 0f;
        private const float jumpDuration = 0.5f;

        public void OnEnter(FP_CharacterController controller)
        {
            controller.Jump();
            elapsed = 0f;
        }

        public void UpdateState(FP_CharacterController controller)
        {
            elapsed += Time.fixedDeltaTime;

            if (elapsed >= jumpDuration)
            {
                controller.SetState(new FP_WalkState());
            }
        }

        public void OnExit(FP_CharacterController controller)
        {
            // Exit logic for jump if needed
        }
    }

    public class FP_CrouchState : IFPControllerState
    {
        public void OnEnter(FP_CharacterController controller)
        {
            controller.SetCrouch(true);
        }

        public void UpdateState(FP_CharacterController controller)
        {
            if (controller.MovementInput.magnitude > 0.01f)
            {
                controller.ApplyMovement(controller.MovementInput * 0.5f); // Optional slower move while crouched
            }

            if (!controller.CrouchInput)
            {
                controller.SetState(new FP_WalkState());
            }
        }

        public void OnExit(FP_CharacterController controller)
        {
            controller.SetCrouch(false);
        }
    }

    public class FP_SlideState : IFPControllerState
    {
        private float elapsed = 0f;
        private const float slideDuration = 1.0f;

        public void OnEnter(FP_CharacterController controller)
        {
            controller.StartSlide();
            elapsed = 0f;
        }

        public void UpdateState(FP_CharacterController controller)
        {
            elapsed += Time.fixedDeltaTime;
            controller.ApplyMovement(controller.MovementInput);

            if (elapsed >= slideDuration || !controller.SlideInput)
            {
                controller.SetState(new FP_WalkState());
            }
        }

        public void OnExit(FP_CharacterController controller)
        {
            controller.StopSlide();
        }
    }
}
