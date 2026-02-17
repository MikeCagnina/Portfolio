# App Icon Setup Instructions

## Icon Design
An SVG icon has been created at `NumberGuessingGame/AppIcon.svg` featuring:
- A blue gradient background
- A green circle with a question mark (representing the guessing game)
- Target rings around it
- Numbers (42, 7, 99, 13) positioned around the circle

## Converting to PNG Files

You need to convert the SVG to PNG files in the following sizes:

### iPhone Icons:
- 40x40 (AppIcon-20x20@2x.png)
- 60x60 (AppIcon-20x20@3x.png)
- 58x58 (AppIcon-29x29@2x.png)
- 87x87 (AppIcon-29x29@3x.png)
- 80x80 (AppIcon-40x40@2x.png)
- 120x120 (AppIcon-40x40@3x.png)
- 120x120 (AppIcon-60x60@2x.png)
- 180x180 (AppIcon-60x60@3x.png)

### iPad Icons:
- 20x20 (AppIcon-20x20@1x.png)
- 40x40 (AppIcon-20x20@2x.png)
- 29x29 (AppIcon-29x29@1x.png)
- 58x58 (AppIcon-29x29@2x.png)
- 40x40 (AppIcon-40x40@1x.png)
- 80x80 (AppIcon-40x40@2x.png)
- 76x76 (AppIcon-76x76@1x.png)
- 152x152 (AppIcon-76x76@2x.png)
- 167x167 (AppIcon-83.5x83.5@2x.png)

### App Store Icon:
- 1024x1024 (AppIcon-1024x1024.png)

## How to Convert

### Option 1: Using Online Tools
1. Use an online SVG to PNG converter (like CloudConvert, Convertio, or SVG2PNG)
2. Upload `AppIcon.svg`
3. Convert to each required size
4. Save each PNG with the correct filename in this folder

### Option 2: Using Image Editing Software
1. Open `AppIcon.svg` in Adobe Illustrator, Inkscape, or similar
2. Export as PNG at each required size
3. Save with the correct filename in this folder

### Option 3: Using Command Line (if you have ImageMagick or similar)
You can use ImageMagick or other command-line tools to batch convert:
```bash
# Example with ImageMagick (if installed)
magick AppIcon.svg -resize 1024x1024 AppIcon-1024x1024.png
# Repeat for each size
```

## Notes
- All icons should have transparent backgrounds (the SVG has rounded corners)
- The icon design uses a blue gradient background with a green circle and question mark
- Make sure all PNG files are placed directly in the `AppIcon.appiconset` folder
- The `Contents.json` file is already configured with all the required sizes

