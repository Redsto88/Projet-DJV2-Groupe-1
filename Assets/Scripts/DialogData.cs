using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Emotion //TODO maybe more
{
    Neutral,
    Annoyed,
    Angry,
    Puzzled,
    Laughing,
    Sad,
    Joy,
    Proud
}

public enum DialogEventType
{
    Fade,
    Appear,
    InLight,
    OutLight,
    Move,
    Swap,
    Talk
}

public enum TransitionType
{
    None,
    Linear,
    Hyperbolic
}

[System.Serializable]
public class DialogEvent
{
    public DialogEventType type;
    public bool IsMove => type == DialogEventType.Move;
    public bool IsSwap => type == DialogEventType.Swap;
    public bool IsTalk => type == DialogEventType.Talk;
    public bool IsntTalk => type != DialogEventType.Talk;
    [Tooltip("The index of the character performing the action.")]
    public int characterIndex;
    
    [NaughtyAttributes.MinValue(0f)]
    [NaughtyAttributes.MaxValue(1f)]
    [NaughtyAttributes.ShowIf("type", DialogEventType.Move)]
    [NaughtyAttributes.AllowNesting]
    [Tooltip("The wanted position relative to the screen. This value is clamped between 0 and 1.")]
    public float position;
    [Tooltip("The index of the character with whom he will swap places.")]
    [NaughtyAttributes.ShowIf("type", DialogEventType.Swap)]
    [NaughtyAttributes.AllowNesting]
    public int otherCharacterIndex;
    
    [NaughtyAttributes.ShowIf(NaughtyAttributes.EConditionOperator.Or, "IsMove", "IsSwap")]
    [NaughtyAttributes.AllowNesting]
    [Tooltip("Defines the movement type.\nNone is instantaneous.")]
    public TransitionType transitionType;
    
    [NaughtyAttributes.ShowIf("IsntTalk")]
    [NaughtyAttributes.AllowNesting]
    [Tooltip("The time the action will take to finish.")]
    public float transitionTime;
    
    [NaughtyAttributes.ShowIf("type", DialogEventType.Talk)]
    [NaughtyAttributes.AllowNesting]
    [Tooltip("The text that will be displayed in the box.")]
    public string text;
    
    [NaughtyAttributes.ShowIf("type", DialogEventType.Talk)]
    [NaughtyAttributes.AllowNesting]
    [Tooltip("Should this dialog produce sound ?")]
    public bool makesSound;
    
    [NaughtyAttributes.ShowIf("makesSound")]
    [NaughtyAttributes.AllowNesting]
    [Tooltip("The base sound each letter will produce.")]
    public AudioClip sound;
    [Tooltip("The ")]
    [NaughtyAttributes.ShowIf("makesSound")]
    [NaughtyAttributes.MinMaxSlider(-2f,2f)]
    [NaughtyAttributes.AllowNesting]
    public Vector2 toneVariation;
    
    [NaughtyAttributes.ShowIf("type", DialogEventType.Talk)]
    [NaughtyAttributes.AllowNesting]
    [Tooltip("The speed at which a new letter appears in milliseconds.")]
    public float textSpeed;
    
    [NaughtyAttributes.ShowIf("type", DialogEventType.Talk)]
    [NaughtyAttributes.AllowNesting]
    [Tooltip("The emotion displayed by the character.")]
    public Emotion emotion;
}


[CreateAssetMenu(fileName = "New Dialog", menuName = "Game/Dialogs/Dialog")]
public class DialogData : ScriptableObject
{
    [Tooltip("The characters interacting in the dialog.")]
    public List<InitCharacters> characters;
    [Tooltip("The list of events occuring in the dialog.")]
    public List<DialogEvent> events;
}

[System.Serializable]
public class InitCharacters
{
    public DialogCharacter character;
    [NaughtyAttributes.MinValue(0f)]
    [NaughtyAttributes.MaxValue(1f)]
    [Tooltip("The position where the character will appear. This value is clamped between 0 and 1.")]
    public float basePosition;
    [Tooltip("The emotion displayed at first by the character.")]
    public Emotion baseEmotion;
    [Tooltip("Is the character in the light when he appears ?")]
    public bool IsInLight;

}