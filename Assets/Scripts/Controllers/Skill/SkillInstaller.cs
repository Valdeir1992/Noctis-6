using System.Collections.Generic;
using Zenject;

public class SkillInstaller : MonoInstaller
{
    private List<Skill> _listSkill;
    public override void InstallBindings()
    {
        Container.Bind<SkillController>().FromMethod(SetupSkill).AsCached();
    } 
    public SkillController SetupSkill()
    {
        _listSkill = new List<Skill>()
        {
            new Skill(){Name = "Fogo vivo", Unlock = false,XP = 0,Type = Skill.UnlockType.HISTORY},
            new Skill(){Name = "Fogo negro", Unlock = true,XP = 0,Type = Skill.UnlockType.HISTORY},
            new Skill(){Name = "Água sagrada", Unlock = false,XP = 0,Type = Skill.UnlockType.HISTORY},
            new Skill(){Name = "Fluido profano", Unlock = false,XP = 0,Type = Skill.UnlockType.HISTORY},
            new Skill(){Name = "Sopro de vida", Unlock = false,XP = 0,Type = Skill.UnlockType.HISTORY},
            new Skill(){Name = "Vento da tormenta", Unlock = false,XP = 0,Type = Skill.UnlockType.HISTORY},
        };
        return new SkillController(_listSkill, 10, 10);
    }
}
