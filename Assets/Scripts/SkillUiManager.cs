// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class Skilla
// {
//     public string skillName;
    
//     public System.Action skillAction;
//     public string description;
//     public string type;
//     public string cooldown;

//     public Skilla(string name, System.Action action, string desc, string type, string cd)
//     {
//         skillName = name;
        
//         skillAction = action;
//         description = desc;
//         this.type = type;
//         cooldown = cd;
//     }
// }

// public class SkillUIManager : MonoBehaviour
// {
//     public GameObject skillButtonPrefab; // Button prefab to be assigned in the Inspector
//     public Transform contentPanel; // The content panel of the Scroll View

//     private Dictionary<string, Skill> skillDictionary = new Dictionary<string, Skill>();

//     void Start()
//     {
//         // Example skill data
//         AddSkill(new Skilla("Fireball", Resources.Load<Sprite>("Icons/fireball"), Fireball, "A powerful fireball.", "Magic", "5s"));
//         AddSkill(new Skilla("Ice Blast", Resources.Load<Sprite>("Icons/iceblast"), IceBlast, "A chilling ice blast.", "Magic", "7s"));
//         AddSkill(new Skilla("Heal", Resources.Load<Sprite>("Icons/heal"), Heal, "Heals the target.", "Support", "10s"));

//         CreateSkillButtons();
//     }

//     void AddSkill(Skill skill)
//     {
//         skillDictionary[skill.skillName] = skill;
//     }

//     void CreateSkillButtons()
//     {
//         foreach (KeyValuePair<string, Skill> entry in skillDictionary)
//         {
//             Skill skill = entry.Value;

//             GameObject skillButton = Instantiate(skillButtonPrefab, contentPanel);

//             // Find the texts and button in the prefab
//             Text[] texts = skillButton.GetComponentsInChildren<Text>();
//             Button button = skillButton.GetComponentInChildren<Button>();

//             // Set the texts
//             //texts[0].text = skill.skillName; // LV:고정
//             //texts[1].text = skill.description; // 레벨이 몇인지
//             //texts[2].text = skill.type; // 스킬 이름 
//             //texts[3].text = skill.cooldown; // LevelUp 고정
//            // texts[4].text = "Skill Type: " + skill.type; // 몇골드 드는지
//            // texts[5].text = "Cooldown: " + skill.cooldown; // G 고정 

           
//             // Add the button click event
//             button.onClick.AddListener(() => skill.skillAction());
//         }
//     }

//     public void CastSkill(string skillName)
//     {
//         if (skillDictionary.TryGetValue(skillName, out Skill skill))
//         {
//             skill.skillAction();
//         }
//         else
//         {
//             Debug.LogWarning($"Skill {skillName} not found!");
//         }
//     }

//     void Fireball()
//     {
//         Debug.Log("Cast Fireball!");
//     }

//     void IceBlast()
//     {
//         Debug.Log("Cast Ice Blast!");
//     }

//     void Heal()
//     {
//         Debug.Log("Cast Heal!");
//     }
// }
