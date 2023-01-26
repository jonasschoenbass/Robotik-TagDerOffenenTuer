from PIL import Image
from PIL import ImageDraw
import os

back = 'Img/under.png'
front = 'Img/overlay_2023TagDerOffenerTür.png'

dirPath = 'OutputPictures'
count = 0

for path in os.listdir(dirPath):
    if os.path.isfile(os.path.join(dirPath, path)):
        count += 1

img1 = Image.open(back)

img2 = Image.open(front)

img1.paste(img2, (0,0), mask=img2)

I1 = ImageDraw.Draw(img1)

I1.text((305, 229), f"{count}", fill=(255, 255, 255))

img1.save(f'OutputPictures/{count}.png', 'PNG')
img1.show()