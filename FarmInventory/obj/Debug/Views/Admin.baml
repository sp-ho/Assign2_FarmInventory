   M S B A M L   `   `   `  ����H  DFarmInventory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:FarmInventory.ViewsFarmInventory.Views  5   	       FarmInventory.Views.Admin   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:FarmInventory.Views     5   	     � Title$  Admin��5   	   $	��485��6   $	��803��6$   .��5	      � .+�5
   
   �� ���	#FF696666$��1q�6)   $
��Left=�6=   $	��150��6X   $��
39,270,0,0q�6e   $	��Top=�6y   $	��270��6�   ��5      �� 5       �� Color$ 	#FFBAD0F3��6"    �� Opacity$	 0.5Q�64   5      �� 5   
   ���	#FF696666$��1q�6)   $
��Left=�6=   $	��199��6X   $��	39,65,0,0q�6e   $	��Top=�6x   $	��270��6�   ��5      �� 5      $ 	#FFBAD0F3��6"   $	 0.5Q�64   5      �� 5   
   -   5         ��Name$ tbProductName��5      $
��Left=�6)   $��173,105,0,0q�6D    �� TextWrapping$
 Wrap=�6Y   $	��Top=�6m   $	��120��6�   $��16(�6�   �� 5   
   -   5         ��Name$ lbProductName��5      $��Product name:��6'   $
��Left=�6?   $��	44,97,0,0q�6Z   $	��Top=�6m   $��16(�6�   �� 5   
   -   5        $ tbProductID��5      $
��Left=�6'   $��173,134,0,0q�6B   $
 Wrap=�6W   $	��Top=�6k   $	��120��6�   $��16(�6�   �� 5   
   -   5        $ lbProductID��5      $��Product ID:��6%   $
��Left=�6;   $��
44,130,0,0q�6V   $	��Top=�6j   $��16(�6�   �� 5   
   -   5        $ 
tbAmountKg��5      $
��Left=�6&   $��173,163,0,0q�6A   $
 Wrap=�6V   $	��Top=�6j   $	��120��6�   $��16(�6�   �� 5   
   -   5        $ 
lbAmountKg��5      $��Amount (kg):��6$   $
��Left=�6;   $��
45,159,0,0q�6V   $	��Top=�6j   $��16(�6�   �� 5   
   -   5        $ tbPricePerKg��5      $
��Left=�6(   $��173,193,0,0q�6C   $
 Wrap=�6X   $	��Top=�6l   $	��120��6�   $��16(�6�   �� 5   
   -   5        $ lbPricePerKg��5      $��Price (CAD)/kg:��6&   $
��Left=�6@   $��
45,189,0,0q�6[   $	��Top=�6o   $��16(�6�   �� 5   
   -	   5         ��Name$ 	btnInsert��5      $��Insert��6$   $
��Left=�65   $��173,227,0,0q�6P   $	��Top=�6e   $��16(�6}   $	��120��6�   �� 5   
   -
   5        $ lblTitle��5      $��HAPPY FARM INVENTORY��6"   $
��Left=�6A   $��	39,15,0,0q�6\   $	��Top=�6o   $��20(�6�   $
��Bold"�6�   ���	#FF242A8E6�   �� 5   
   -   5        $ 	btnDelete��5      $��Delete��6$   $
��Left=�65   $��
702,62,0,0q�6P   $	��Top=�6d   $��16(�6|    J� RenderTransformOrigin$ 0.555,0.843&�6�   $��61��6�   �� 5   
   -   5        $ 	btnUpdate��5      $��Direct Update��6$   $
��Left=�6<   $��
52,375,0,0q�6W   $	��Top=�6k   $��16(�6�   $	��110��6�   $��28��6�   $��Center=�6�   $��Center=�6�   6�   &   System.Windows.Controls.DataGrid  5    
   -   5          Name$ dataView��5       	   AutoGenerateColumns	 .  $��338,117,26,29q�6A   $��16(�6X   
   Columns
 5!      0  *System.Windows.Controls.DataGridTextColumn  5"      ,  &System.Windows.Controls.DataGridColumn   Header$ Product Name��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *name+  5#      $ 
Product ID��6%    69   �� *id+  5$      $ Amount (kg)��6%    6:   �� *amount+  5%      $ Price(CAD)/kg��6%    6<   �� *price+5'      �� 5(   
   -   5        $ 
btnShowAll��5(      $��Show All��6%   $
��Left=�68   $��
616,62,0,0q�6S   $	��Top=�6g   $��16(�6   $��80��6�   $ 0.864,0.487&�6�   �� 5)   
   -   5        $ 	btnSearch��5)      $��Search��6$   $
��Left=�65   $��
547,62,0,0q�6P   $	��Top=�6d   $��16(�6|   $��62��6�   6�   �� 5*   
   -   5        $ 
tbSearchId��5*      $
��Left=�6&   $��
432,62,0,0q�6A   $
 Wrap=�6U   $	��Top=�6i   $	��106��6�   $��16(�6�   $��25��6�   �� 5+   
   -   5        $ lblTitle_Copy��5+      $��Insert New Product��6'   $
��Left=�6D   $��	44,68,0,0q�6_   $	��Top=�6r   $��16(�6�   $
��Bold"�6�   �� 5,   
   -   5        $ lbProductIDSearch��5,      $��Product ID:��6+   $
��Left=�6A   $��
338,59,0,0q�6\   $	��Top=�6p   $��16(�6�   �� 5-   
   -   5        $ lblDeleteNotes��5-      $��Select a row to delete��6(   $
��Left=�6I   $��
338,86,0,0q�6d   $	��Top=�6x   $��14(�6�   $	��162��6�   $��Italic$�6�   �� 5.   
   -   5        $ lblTitle_Copy1��5.      $��Update Product Amount��6(   $
��Left=�6H   $��
45,273,0,0q�6c   $	��Top=�6w   $��16(�6�   $
��Bold"�6�   �� 5/   
   -   5        $ lbProductIDUpdate��5/      $��Product ID:��6+   $
��Left=�6A   $��
47,306,0,0q�6\   $	��Top=�6p   $��16(�6�   �� 50   
   -   5        $ 
tbIdUpdate��50      $
��Left=�6&   $��173,310,0,0q�6A   $
 Wrap=�6V   $	��Top=�6j   $	��120��6�   $��16(�6�   �� 51   
   -   5        $ lbAmountKgUpdate��51      $��Amount (kg):��6*   $
��Left=�6A   $��
48,335,0,0q�6\   $	��Top=�6p   $��16(�6�   �� 52   
   -   5        $ tbAmountKgUpdate��52      $
��Left=�6,   $��173,340,0,0q�6G   $
 Wrap=�6\   $	��Top=�6p   $	��120��6�   $��16(�6�   �� 53   
   -   5        $ btnPlus��53      $��+��6"   $
��Left=�6.   $��175,375,0,0q�6I   $	��Top=�6^   $��16(�6v   $��58��6�   $��28��6�   $��Center=�6�   $��Center=�6�   6�   �� 54   
   -   5        $ btnMinus��54      $��-��6#   $
��Left=�6/   $��238,375,0,0q�6J   $	��Top=�6_   $��16(�6w   $��55��6�   $��28��6�   $��Center=�6�   $��Center=�6�   6�   �� 55   
   -   5        $ responselbl��55      $��Label��6%   $
��Left=�65   $��
400,15,0,0q�6P   $	��Top=�6d   
���  ��6|   56      57      