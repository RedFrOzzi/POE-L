using System.Text;
using UnityEngine;
using TMPro;


public class EnemyBehaviourDummy : EnemyBehaviour
{
    private TextMeshProUGUI text;

    private bool isSetUp;
    private StringBuilder strBldr = new();

    private float lastHealthStamp;


    public override void Moving(Vector2 movingTo)
    {
        if (isSetUp) { return; }

        text = GameObject.Find("Training_Dummy_UI").GetComponent<TextMeshProUGUI>();
        lastHealthStamp = stats.CurrentHP;
        stats.Health.OnHealthChange += OnHealthChange;
        stats.Health.OnDeath += OnDeath;
    }
    public override void Attack(Vector2 target)
    {
        return;
    }

    private void OnHealthChange(float damage)
    {
        strBldr.Clear();
        strBldr.Append($"Health: {stats.CurrentHP}\n" +
            $"Current Hit: {lastHealthStamp - stats.CurrentHP}\n");
        text.text = strBldr.ToString();
    }

    private void OnDeath()
    {
        stats.Health.OnHealthChange -= OnHealthChange;
        stats.Health.OnDeath -= OnDeath;
    }
}
