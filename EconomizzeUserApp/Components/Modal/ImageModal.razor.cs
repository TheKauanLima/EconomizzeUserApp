using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace EconomizzeUserApp.Components.Modal
{
    /// <summary>
    /// Component to handle the display of modal windows.
    /// </summary>
    public partial class ImageModal
    {
        /// <summary>
        /// Instance of the modal to be shown.
        /// </summary>
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }

        /// <summary>
        /// Image base 64 string to display from memory.
        /// </summary>
        [Parameter] public string? imageString { get; set; }

        /// <summary>
        /// Image url from cloud.
        /// </summary>
        [Parameter] public string? imageUrl { get; set; }
    }
}