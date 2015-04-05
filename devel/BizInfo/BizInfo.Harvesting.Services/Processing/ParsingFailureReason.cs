namespace BizInfo.Harvesting.Services.Processing
{
    public enum ParsingFailureReason
    {
        /// <summary>
        /// Příčina selhání je neznámá
        /// </summary>
        Unknown,

        /// <summary>
        /// Příčinou jsou chybná data. Je možné se pokusit o jejich opětovné získání
        /// </summary>
        /// <remarks>
        /// Typické je poškození dat použitím špatné proxy
        /// </remarks>
        WrongDataTryReload,

        /// <summary>
        /// Příčinou jsou chybná data. Nemá smysl se pokoušet o jejich opětovné získání, data byla ztracena
        /// </summary>
        /// <remarks>
        /// Typické je smazání inzerátu uživatelem
        /// </remarks>
        WrongDataUnrecoverable
    }
}