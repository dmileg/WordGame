﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WordGameAPI.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WordGameAPI.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Submitted word had been used. Try another one.
        /// </summary>
        public static string AlreadySubmitted {
            get {
                return ResourceManager.GetString("AlreadySubmitted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Word is empty. Enter a word..
        /// </summary>
        public static string EmptyWord {
            get {
                return ResourceManager.GetString("EmptyWord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Word should consist of given letters. Each of them could be used only once.
        /// </summary>
        public static string ExtraLettersInSubmission {
            get {
                return ResourceManager.GetString("ExtraLettersInSubmission", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Game is already over. Start a new one..
        /// </summary>
        public static string GameIsNotFound {
            get {
                return ResourceManager.GetString("GameIsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Game is over..
        /// </summary>
        public static string GameOver {
            get {
                return ResourceManager.GetString("GameOver", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Submitted word does not exist. Try another one.
        /// </summary>
        public static string NonexistentWord {
            get {
                return ResourceManager.GetString("NonexistentWord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nice job!.
        /// </summary>
        public static string Success {
            get {
                return ResourceManager.GetString("Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Something went wrong. Try again later..
        /// </summary>
        public static string UnknownException {
            get {
                return ResourceManager.GetString("UnknownException", resourceCulture);
            }
        }
    }
}