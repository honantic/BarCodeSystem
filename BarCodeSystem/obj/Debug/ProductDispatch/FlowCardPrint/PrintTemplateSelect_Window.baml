   M S B A M L   `   `   `  ����  BarCodeSystemL    FBarCodeSystem.ProductDispatch.FlowCardPrint.PrintTemplateSelect_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ 打印模板选择窗口��5   	   $	��300��6   $	��600��6'   .��5      � -   5        $��grid_FatherGrid��5       � RowDefinitions 5
   
   �� 5      $��30 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� $��3��6   .��5      �� -   5        $��textb_Header��5   &   $��15(�6   
�������打印模板选择6U   ��5      �� 5      ��5      �  � Offset$ 1Q�6(    � Color$ 
LightGreen��63   6H   � 5      $ Green��6'   67   5      5      �� 5   
   -   5        $��listb_PrintTemplate��5   W   $��1��6   $��5q�6r   $��Center=�6}   $��Center=�6�   ��5      �� 5       �� Color$ 	LightBlue��6"    �� Opacity$	 0.9Q�64   6D   ��5      �� 5       .R�5!      �� $��Disabled=�6#   $
��Auto=�6L   .��5"      �  � Orientation$ 
Horizontal=�6$   	 K� IsItemsHost	 . ��. 6P   5#      5$      .@�5&      �� -   5        $��_10LinesTemplate��5&   3   $	��100��6   $	��100��6'   
������$��Center=�6d   ��5'      �� 5(      .+�5)      �� 10行模板6+   5*      5,      �� 5-      -   5        $��_20LinesTemplate��5-   3   $	��100��6   $	��100��6'   
����ح�$��Center=�6b   ��5.      �� 5/      .+�50      �� 20行模板6+   51      53      54      � 56   
   $��2��6   
 � ColumnDefinitions
 57      �� 58      6%   �� 59      6%   .+�5;      �� -   5        $��btn_SelectTemplate��5;   :   $��   选定打印模板  ��6   $
��25 5q�6+   $��Center=�6U   $��Center=�6p    �� 	ClickMode$ Release=�6�   6�   �� 5<      -   5        $��	btn_Close��5<   v   $��   关       闭  ��6   $
��25 5q�6/   $��Center=�6>   $��Center=�6Y   $ Release=�6�   $��1��6�   6�   5=      5>      5@      