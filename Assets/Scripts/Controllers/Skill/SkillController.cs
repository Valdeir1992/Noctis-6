using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class SkillController 
{
    private int _nightmareXP;
    private int _dreamXP; 
    private List<Skill> _listSkill;

    public Tuple<string,bool>[] ListSkill 
    { 
        get => _listSkill.Select(x => new Tuple<string, bool>(x.Name,x.Unlock)).ToArray(); 
    }
    public int NightmareXP { get => _nightmareXP;}
    public int DreamXP { get => _dreamXP;}
    public int TotalXP { get => _dreamXP+_nightmareXP;}

    public enum SkillType
    {
        NIGHTMARE,
        DREAM
    }
    public SkillController(List<Skill> list, int nightmareXP, int dreamXP)
    {
        _listSkill = list;
        _dreamXP = dreamXP;
        _nightmareXP = nightmareXP;
    }
    public void AddXP(int amount,SkillType point)
    {
        switch (point)
        {
            case SkillType.NIGHTMARE:_nightmareXP += amount; break;
            case SkillType.DREAM: _dreamXP += amount;break;
        }
    }
    public bool CheckXPSkill(string skillname, int xp)
    {
        return _listSkill.Any(x => x.Name == skillname && x.XP <= xp);
    }
    public void OpenSkill(Skill skill)
    {
        var selected = _listSkill.First(x => x.Name == skill.Name);
        selected.Unlock = true;
    }
}
public class Skill
{
    public string Name;
    public int XP;
    public UnlockType Type;
    public bool Unlock;
    public string Description;

    public enum UnlockType
    {
        XP,
        HISTORY,
    }
}
