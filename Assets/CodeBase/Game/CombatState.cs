namespace CodeBase.Game
{
    public enum CombatState
    {
        PlayerDraw,     //EnergyMax,Remove Block, DrawCards 
        PlayerInput,    // WAITING FOR PLAYER TO CLICK CARDS/END TURN
        PlayerEnd,      // Discard hand
        EnemyExecute,   // Enemies perform their actions (queued up)
        EnemyEnd,       // Remove temporary enemy buffs (e.g., Block)
        BattleEnd       // Win or Lose
    }
}