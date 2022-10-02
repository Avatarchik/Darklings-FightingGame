using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
[CreateAssetMenu(fileName = "AnimationSO", menuName = "Darklings-FightingGame/AnimationSO", order = 0)]
public class AnimationSO : ScriptableObject
{
    public SpriteAtlas[] spriteAtlas;
    public AnimationCelsGroup[] animationCelsGroup;

    public Sprite GetSprite(int skin, int group, int cel)
    {
        return spriteAtlas[skin].GetSprite(animationCelsGroup[group].animationCel[cel].sprite.name);
    }

    public AnimationCelsGroup GetGroup(int group)
    {
        return animationCelsGroup[group];
    }

    public AnimationCel GetCel(int group, int cel)
    {
        return animationCelsGroup[group].animationCel[cel];
    }

    public int GetGroupId(string name)
    {
        for (int i = 0; i < animationCelsGroup.Length; i++)
        {
            if (animationCelsGroup[i].celName == name)
            {
                return i;
            }
        }
        return 0;
    }
}

[Serializable]
public struct AnimationCelsGroup
{
    public string celName;
    public bool loop;
    public List<AnimationCel> animationCel;
}

[Serializable]
public class AnimationCel
{
    public int frames;
    public Sprite sprite;
    public List<AnimationBox> hitboxes;
    public List<AnimationBox> hurtboxes;
}

[Serializable]
public class AnimationBox
{
    public Vector2 size;
    public Vector2 offset;
}