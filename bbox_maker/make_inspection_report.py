"""
Make inspection report from command
"""
import os
import pandas as pd
import cv2
import numpy
import matplotlib.pyplot as plt
from matplotlib.backends.backend_pdf import PdfPages
from matplotlib.offsetbox import OffsetImage, AnnotationBbox

def make_report_from_logs(defect_log_path, logo_file_path, save_pdf_path):

    df = pd.read_csv(defect_log_path)

    # create a sample figure with two subplots
    fig, axs = plt.subplots(nrows=len(df)+1, ncols=2, figsize=(20, int(3*len(df))))

    # add TSI logo as offset image

    logo_r = cv2.imread(logo_file_path)
    logo = cv2.cvtColor(logo_r, cv2.COLOR_BGR2RGB)

    for j in range(2):
        
        axs[0,j].imshow(logo)
        axs[0,j].tick_params(axis='both', which='both', length=0, labelsize=0)
        # remove the border
        axs[0,j].spines['top'].set_visible(False)
        axs[0,j].spines['right'].set_visible(False)
        axs[0,j].spines['bottom'].set_visible(False)
        axs[0,j].spines['left'].set_visible(False)

    for i, row in df.iterrows():
        
        datetime = row['datetime']
        image_path = row['image_path']
        notes = row['notes']
        x = row['loc_x']
        y = row['loc_y']
        label = row['defect']
        def_h = row['def_h']
        def_w = row['def_w']
        image_index = row['image_index']
        pipe_ID = row['pipe_ID']
        inspector = row['inspector']

        # open img, find defect
        img = cv2.imread(image_path)    
        defect = img[y:y+def_h,x:x+def_w,:]

        axs[i+1,0].imshow(defect)
        
        axs[i+1,1].text(0.0,0.9, "Image Name: " + image_path)
        axs[i+1,1].text(0.0,0.8, "Datetime: " + datetime)
        axs[i+1,1].text(0.0,0.7, "Pipe ID: " + pipe_ID)
        axs[i+1,1].text(0.0,0.6, "Inspector: " + inspector)
        axs[i+1,1].text(0.0,0.5, "Defect: " + label)
        axs[i+1,1].text(0.0,0.4, "Notes: " + notes)

        # remove ticks
        axs[i+1,0].tick_params(axis='both', which='both', length=0, labelsize=0)
        axs[i+1,1].tick_params(axis='both', which='both', length=0, labelsize=0)
    
        # remove the border
        axs[i+1,0].spines['top'].set_visible(False)
        axs[i+1,0].spines['right'].set_visible(False)
        axs[i+1,0].spines['bottom'].set_visible(False)
        axs[i+1,0].spines['left'].set_visible(False)
        
        # remove the border
        axs[i+1,1].spines['top'].set_visible(False)
        axs[i+1,1].spines['right'].set_visible(False)
        axs[i+1,1].spines['bottom'].set_visible(False)
        axs[i+1,1].spines['left'].set_visible(False)



    # create a PdfPages object to save the plots to a PDF file
    with PdfPages(save_pdf_path) as pdf:
        # save each subplot to the PDF file
        pdf.savefig(fig)

    plt.show()



if __name__ == "__main__":
    
    root = os.getcwd()
    print('root: ', root)

    defect_log_path = r'C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\defectslog.txt'
    save_pdf_path = r'C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\inspection_report.pdf'
    logo_file_path = r'C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\logoTUBES.png'

    make_report_from_logs(defect_log_path, logo_file_path, save_pdf_path)
