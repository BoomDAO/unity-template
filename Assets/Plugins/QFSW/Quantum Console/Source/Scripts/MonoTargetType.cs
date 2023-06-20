﻿namespace QFSW.QC
{
    /// <summary>
    /// Determines the target type for non static MonoBehaviour commands.
    /// </summary>
    public enum MonoTargetType
    {
        /// <summary>
        /// Targets the first instance found of the MonoBehaviour.
        /// </summary>
        Single = 0,

        /// <summary>
        /// Targets all instances found of the MonoBehaviour.
        /// </summary>
        All = 1,

        /// <summary>
        /// Targets all instances registered in the QuantumRegistry. Instances can be added using <c>QFSW.QC.QuantumRegistry.RegisterObject</c>.
        /// The only supported target type for non MonoBehaviour commands
        /// </summary>
        Registry = 2,

        /// <summary>
        /// Automatically creates an instance if it does not yet exist and adds it to the registry, where it will be used for all future function calls.
        /// </summary>
        Singleton = 3,

        /// <summary>
        /// Targets the first instance found of the MonoBehaviour. Includes inactive objects in its search.
        /// </summary>
        SingleInactive = 4,

        /// <summary>
        /// Targets all instances found of the MonoBehaviour. Includes inactive objects in its search.
        /// </summary>
        AllInactive = 5,

        /// <summary>
        /// The instance targeted will be specified as the first argument in the command invocation.
        /// </summary>
        Argument = 6,

        /// <summary>
        /// The instances targeted will be specified by an array as the first argument in the command invocation.
        /// </summary>
        ArgumentMulti = 7
    }
}