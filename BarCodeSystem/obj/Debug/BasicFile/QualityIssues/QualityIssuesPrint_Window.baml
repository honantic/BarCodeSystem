   M S B A M L   `   `   `  ����  BarCodeSystemE    ?BarCodeSystem.BasicFile.QualityIssues.QualityIssuesPrint_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ QualityIssuesPrint_Window��5	   	   $	��750��6+   $
��1050��68    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6E   .��5
      �  � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� 
���   �$��1q�6%   $��3��69   $��0��6J   6Y   �� 5   
   $��3��6   .��5      �� -   5        $��textb_Header��5   &   $��15(�6   
�������打印内容预览6U   ��5      �� 5      ��5      �  � Offset$ 1Q�6(    � Color$ 
LightGreen��63   6H   � 5      $ Green��6'   67   5      5      � 5   
   -   5        $��grid_CenterGrid��5      $��1��6   .+�5       �� -   5        $��listb_PrintTemplate��5    #   $��1��6   $��5q�6>   ��5!      �� 5"       �� Color$ 	LightBlue��6&   	 �� Opacity$		 0.9Q�68   6H   ��5$      �� 5%      .R�5&      �� $��Disabled=�6'   $
��Auto=�6P   .��5'      � 
 � Orientation$
 
Horizontal=�6(    K� IsItemsHost . ��. 6T   5(      5)      5+      5,      � 5/   
   $��2��6    � ColumnDefinitions 50      �� 51      6%   �� 52      6%   .+�54      �� -   5        $��	btn_Print��54      $��      打   印      ��6&   $��Center=�6Z   $��Center=�6u   6�   �� 55      -   5        $��	btn_Close��55      $��      关   闭      ��6&   $��Center=�6Z   $��Center=�6u   $��1��6�   6�   57      58      59      