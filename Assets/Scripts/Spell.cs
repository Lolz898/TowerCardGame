using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellTarget
{
    Single,
    AoE
}

public class Spell : MonoBehaviour
{
    public SpellTarget spellTarget = SpellTarget.Single;
    public bool hitAlly = false;
    public bool hitEnemy = false;
    public bool hitTower = false;
    public int damage = 10;
    public int range = 1;
    public int manaCost = 1;

    private void Start()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in collidersInRange)
        {
            Unit unit = collider.GetComponent<Unit>();
            Tower tower = collider.GetComponent<Tower>();

            if (unit != null)
            {
                if ((hitAlly && !unit.isEnemy) || (hitEnemy && unit.isEnemy))
                {
                    CastSpell(unit);
                }
            }

            if (tower != null && hitTower)
            {
                CastSpellOnTower(tower);
            }
        }

        Destroy(gameObject);
    }

    public void CastSpell(Unit target)
    {
        if (damage != 0)
        {
            target.TakeDamage(damage);
        }
    }

    public void CastSpellOnTower(Tower target)
    {
        Debug.Log("Spell casted on tower");
    }
}