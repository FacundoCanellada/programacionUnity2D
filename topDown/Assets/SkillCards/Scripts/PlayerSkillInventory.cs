using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillInventory : MonoBehaviour
{
    public int cardCapacity = 4; // Capacidad por tipo de carta

    // Diccionario para guardar las cartas adquiridas. Key: nombre de la carta, Value: objeto SkillCard
    private Dictionary<string, SkillCard> acquiredSkills = new();

    // Agrega una carta o sube de nivel si ya la tiene
    public void AddOrLevelUpSkill(SkillCard skill) {
        string skillName = skill.cardName;

        if (acquiredSkills.ContainsKey(skillName)) {
            acquiredSkills[skillName].LevelUp();
            Debug.Log($"🔁 '{skillName}' subió a nivel {acquiredSkills[skillName].currentLevel}");
        } else {
            // Validación por seguridad, aunque normalmente esto ya estaría filtrado
            if (!CanAddSkillOfType(skill.cardType)) {
                Debug.LogWarning($"❌ No se puede agregar '{skillName}'. Límite de tipo {skill.cardType} alcanzado.");
                return;
            }

            SkillCard newSkill = Instantiate(skill);
            newSkill.currentLevel = 1;
            acquiredSkills.Add(skillName, newSkill);
            Debug.Log($"🆕 '{skillName}' agregada al inventario. Nivel: {newSkill.currentLevel}");
        }
    }

    // Devuelve true si se puede agregar una carta del tipo dado (Stat o Ability), basado en la cantidad actual
    public bool CanAddSkillOfType(CardType type) {
        int count = 0;
        foreach (var skill in acquiredSkills.Values) {
            if (skill.cardType == type) count++;
        }
        return count < cardCapacity;
    }

    // Devuelve true si ya se tiene una carta con ese nombre
    public bool HasSkill(string name) => acquiredSkills.ContainsKey(name);

    // Devuelve la carta con ese nombre, si existe
    public SkillCard GetSkill(string name) {
        acquiredSkills.TryGetValue(name, out var skill);
        return skill;
    }

    // Devuelve todas las cartas adquiridas
    public Dictionary<string, SkillCard> GetAllSkills() => acquiredSkills;
}