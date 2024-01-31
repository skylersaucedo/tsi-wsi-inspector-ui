"""
Use this to populate folder with stills
"""

# convert vid to stills
import os
import cv2
import sys
from distutils.dir_util import copy_tree
import shutil
from pathlib import Path
from tqdm import tqdm
import boto3

client = boto3.client('s3', region_name = 'us-east-2')
bucket = 'tsi-mlops'


def make_stills_from_vid(vid_path, save_path, project_name, BoxOrPin, passnum, camnum):
    """
    define vid path & save path, output folder of images in save path
    """
    video = cv2.VideoCapture(vid_path)
    image_list = []

    # Initialize a frame counter
    frame_count = 0
    
    if (os.path.exists(save_path) == False):
        os.mkdir(save_path)

    # Loop through the video frames
    while True:
        # Read the next frame
        ret, frame = video.read()

        # If there are no more frames, break out of the loop
        if not ret:
            break

        # Save the current frame as an image
        
        #img_name = save_path + "\\" + f'{frame_count:03}_img.png'
        img_name = save_path + "\\" + project_name + "_" + BoxOrPin + "_" + passnum + "_" + camnum +"_"+ f'{frame_count:03}_img.bmp'
        #img_name = save_path + "\\" + f'{frame_count:03}_img.jpg'

        # rotate 90 degs for UI?

        frame = cv2.rotate(frame, cv2.ROTATE_90_COUNTERCLOCKWISE)

        cv2.imwrite(img_name, frame)

        # add upload to s3 here

        client.upload_file(img_name, bucket, img_name)



        image_list.append(img_name)

        # Increment the frame counter
        frame_count += 1

    # Release the video object
    video.release()
    
    return image_list


def main(root_vid_pth):
    print('root vid path: ', root_vid_pth)

    # 1. copy original project file to local drive

    project_scan_path = r"C:\wsi-project-scans"

    # isolate folder name from root vid path

    project_name = os.path.basename(os.path.normpath(root_vid_pth))

    print('project name: ', project_name)
    print('copying files over.. please wait a moment...')

    # make new project name folder

    new_project_folder_path = project_scan_path + "\\" + project_name

    if os.path.exists(new_project_folder_path) is False:

        os.mkdir(new_project_folder_path)

        # make backup on HMI

        copy_tree(root_vid_pth, new_project_folder_path)

    # 2. find avis to convert to imgs

    avi_files = []
    for root, dirs, files in os.walk(new_project_folder_path):
        for file in tqdm(files):
            if file.endswith('.avi'):
                avi_files.append(os.path.join(root, file))

    print(avi_files)

    # we cound video paths, now we need to create project folder structure and

    for f in tqdm(avi_files):
        splity = f.split("\\")

        # box or pin

        BoxOrPin = ''
        if any('BOX' in x  for x in splity):
            BoxOrPin = 'BOX'

        else:
            BoxOrPin = 'PIN'

        # pass number

        passnum = ''
        for i in range(6):
            if any('pass{}'.format(i) in x  for x in splity):
                passnum = 'pass{}'.format(i)

        # cam number

        camnum = ''
        for j in tqdm(range(4)):
            if any('cam{}'.format(j) in x  for x in splity):
                camnum = 'cam{}'.format(j)

        print('box or pin: ', BoxOrPin)
        print('pass num: ', passnum)
        print('cam num: ', camnum)

        stills_path = new_project_folder_path + '\\' 'RAW' + "\\" + BoxOrPin + '\\' + passnum + '\\' + camnum + '\\' + 'stills'

        print('making stills here: ', stills_path)

        if not os.path.exists(stills_path):
            os.makedirs(stills_path)

        is_empty = not bool({_ for _ in Path(stills_path).rglob('*.bmp')}) # images already exist? it's possible...

        if is_empty:
                
            img_list_full = make_stills_from_vid(f, stills_path, project_name, BoxOrPin, passnum, camnum)

            print(img_list_full)

            print('moving video to folder: ')

            videopath = new_project_folder_path + '\\' 'RAW' + "\\" + BoxOrPin + '\\' + passnum + '\\' + camnum + '\\' + 'vids'
            os.makedirs(videopath)
            videofilename = splity[-1]
            shutil.move(f, videopath + "\\" + videofilename)

        else:
            print('images already generated, moving on to next folder...')


if __name__ == "__main__":

    root_vid_pth = sys.argv[1]
    main(root_vid_pth)
    print('all stills generated for vids.')


