   M S B A M L   `   `   `  ����H  DFarmInventory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:FarmInventory.ViewsFarmInventory.Views  5   	       FarmInventory.Views.Sales   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:FarmInventory.Views     5   	     � Title$  Sales��5   	   $	��433��6   $	��959��6$   .��5	      � .+�5   
   �� -   5         ��Name$ listBoxProducts��5      $��47,60,755,97q�6+    �� DisplayMemberPath$
 name��6A   $��16(�6Z   �� 5   
   -   5         ��Name$ lblAmountKg��5      $��Amount (kg)��6%   $
��Left=�6;   $��228,115,0,0q�6V   $	��Top=�6k   $��20(�6�   $��Center=�6�   $	��145��6�   �� 5   
   -   5         ��Name$ tbAmount��5      $
��Left=�6$   $��228,155,0,0q�6?    �� TextWrapping$
 Wrap=�6T   $
��0.00��6h   $	��Top=�6t   $	��145��6�   $��30(�6�   $��Center=�6�   $��48��6�   $��Center=�6�   �� 5   
   -   5         ��Name$ btnAddToCart��5      $��Add To Cart��6'   $
��Left=�6=   $��242,217,0,0q�6X   $	��Top=�6m   $��20(�6�   $	��118��6�    J� RenderTransformOrigin$ 
0.53,2.265&�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGridCart��5      $��400,60,50,100q�6)   $��16(�6@   	   AutoGenerateColumns	 .  
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Product name��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *productName+  5      $ 
Product ID��6%    69   �� *	productId+  5      $ Amount (kg)��6%    6:   �� *amountPurchased+  5      $ Price(CAD)/kg��6%    6<   �� *
pricePerKg+  5      $ Subtotal��6%    67   �� *
subtotal+5      �� 5   
   -   5        $ lblTotalPrice��5      $��$0.00��6'   $
��Left=�67   $��663,320,0,0q�6R   $	��Top=�6g   $��24(�6   $	��109��6�   �� 5   
   -   5        $ lblTotal��5      $��Total Price: ��6"   $
��Left=�6:   $��521,320,0,0q�6U   $	��Top=�6j   $��24(�6�   $	��137��6�   �� 5   
   -   5        $ 
btnConfirm��5      $��Confirm��6%   $
��Left=�67   $��799,325,0,0q�6R   $	��Top=�6g   $��20(�6   $��84��6�   6�   5      5      