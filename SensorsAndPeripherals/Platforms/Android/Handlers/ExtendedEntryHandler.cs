using Android.Graphics;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SensorsAndPeripherals.Controls;
using SensorsAndPeripherals.Helpers;
using BlendMode = Android.Graphics.BlendMode;

namespace SensorsAndPeripherals.Platforms.Android.Handlers
{
    public class ExtendedEntryHandler : EntryHandler
    {
        private new readonly static IPropertyMapper<ExtendedEntry, ExtendedEntryHandler> Mapper
           = new PropertyMapper<ExtendedEntry, ExtendedEntryHandler>(EntryHandler.Mapper)
           {
               [nameof(ExtendedEntry.UnderlineColor)] = MapUnderlineColor,
               [nameof(ExtendedEntry.CursorColor)] = MapCursorColor
           };

        public ExtendedEntryHandler() : base(Mapper)
        {
        }

        protected override void ConnectHandler(MauiAppCompatEditText platformView)
        {
            base.ConnectHandler(platformView);
            platformView.SetHighlightColor("Gray200".SafeGetResource<Microsoft.Maui.Graphics.Color>().ToPlatform());
        }

        private static void MapUnderlineColor(ExtendedEntryHandler handler, ExtendedEntry entry)
        {
            if (!CanBeUpdated(handler)) return;
            if (handler.PlatformView is AppCompatEditText editText && editText.Background is not null)
            {
                var androidColor = entry.UnderlineColor.ToPlatform();
                if (OperatingSystem.IsAndroidVersionAtLeast(29))
                {
                    editText.Background.Mutate().SetColorFilter(new BlendModeColorFilter(androidColor, BlendMode.SrcIn!));
                }
                else
                {
                    editText.Background.Mutate().SetColorFilter(androidColor, PorterDuff.Mode.SrcIn!);
                }
            }
        }

        public static void MapCursorColor(ExtendedEntryHandler handler, ExtendedEntry entry)
        {
            if (!CanBeUpdated(handler)) return;
            if (handler.PlatformView is EditText editText && OperatingSystem.IsAndroidVersionAtLeast(29))
            {
                editText.TextCursorDrawable?.Mutate().SetTint(entry.CursorColor.ToPlatform());
                editText.TextSelectHandle?.Mutate().SetTint(entry.CursorColor.ToPlatform());
                editText.TextSelectHandleLeft?.Mutate().SetTint(entry.CursorColor.ToPlatform());
                editText.TextSelectHandleRight?.Mutate().SetTint(entry.CursorColor.ToPlatform());
            }
        }

        private static bool CanBeUpdated(ExtendedEntryHandler handler)
        {
            try
            {
                return handler?.PlatformView is not null;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }
    }
}