using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSliderComponent : MonoBehaviour
{
    public enum SliderPositions
    {
        Left,
        Center,
        Right
    }

    [SerializeField]
    private MainMenuGuiController guiController;

    [SerializeField]
    private Transform positionLeft;

    [SerializeField]
    private Transform positionCenter;

    [SerializeField]
    private Transform positionRight;

    [SerializeField]
    private Transform deviceImage;

    [SerializeField]
    private int cooldownFrames;

    private int cooldownFramesCounter;

    private SliderPositions currentPosition = SliderPositions.Center;

    private bool isReadyToSlide { get { return this.cooldownFrames <= this.cooldownFramesCounter; } }

    public SliderPositions CurrentPositon { get { return this.currentPosition; } }


    public void Update()
    {
        if(this.cooldownFramesCounter < this.cooldownFrames)
        {
            this.cooldownFramesCounter++;
        }
    }

    public void MoveSliderInDirection(SliderPositions direction)
    {
        if (this.isReadyToSlide)
        {
            switch (direction)
            {
                case SliderPositions.Left:
                    switch (this.currentPosition)
                    {
                        case SliderPositions.Center:
                            this.MoveSlider(SliderPositions.Left);
                            break;
                        case SliderPositions.Right:
                            this.MoveSlider(SliderPositions.Center);
                            break;
                    }
                    break;
                case SliderPositions.Right:
                    switch (this.currentPosition)
                    {
                        case SliderPositions.Center:
                            this.MoveSlider(SliderPositions.Right);
                            break;
                        case SliderPositions.Left:
                            this.MoveSlider(SliderPositions.Center);
                            break;
                    }
                    break;
            }
        }
    }

    private void MoveSlider(SliderPositions newPosition)
    {
        switch (newPosition)
        {
            case SliderPositions.Left:
                this.deviceImage.position = this.positionLeft.position;
                break;
            case SliderPositions.Center:
                this.deviceImage.position = this.positionCenter.position;
                break;
            case SliderPositions.Right:
                this.deviceImage.position = this.positionRight.position;
                break;
        }

        this.currentPosition = newPosition;
        this.cooldownFramesCounter = 0;
        this.guiController.SetControlsPlayerPref();
    }

}
