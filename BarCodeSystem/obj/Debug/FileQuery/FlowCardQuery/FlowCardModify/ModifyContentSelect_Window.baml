   M S B A M L   `   `   `  ����  BarCodeSystemF    @BarCodeSystem.FileQuery.FlowCardQuery.ModifyContentSelect_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   -   5          � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$! 选择流转卡修改内容��5
   	   $	��200��6   $	��400��6(    � Icon$. (/BarCodeSystem;component/Images/burn.ico��64   .��5      � .+�5   
   �� 
����ح�$��4��6)   $��2��6:   $��10q�6N   6e   � 5   
   -   5        $��grid_ContentSelectGrid��5       � RowDefinitions 5      �� 5      $
��0.5* �6    60   �� 5      6"   �� 5      6#    � ColumnDefinitions 5      �� 5      6%   �� 5      6%   .+�5      �� $��请选择修改内容��6   $��2��6'   $��Bottom=�6;   $��Center=�6V   6u   �� 5      -   5        $��btn_ModifyFCAmount��5   P   $��1��6   $	��120��6"   $��25��61   $��修改派工数量->��6=    �� 	ClickMode$ Release=�6�   6�   �� 5      -   5        $��btn_ModifyReportInfo��5   `   $��1��6   $��1��6"   $	��120��62   $��25��6A   $��修改报工信息->��6M   $ Release=�6�   6�   �� 5      -   5        $��
btn_Cancel��5   {   $��3��6   $��Center=�6#   $��2��6>   $��25��6S   $	��120��6_   $��取消��6n   $ Release=�6�   6�   5      � 5   
   -   5        $��grid_FCAmountModifyGrid��5      $y�Hidden=�6/    5      �� 5      $
��0.5* �6    60   �� 5       6"   �� 5!      6#    5#      �� 5$      6%   �� 5%      6%   .+�5'      �� $��修改派工数量��6   $��2��6&   $��Bottom=�6:   $��Center=�6U   6t   �� 5(      $��10 0 0 0q�6   $��1��6+    �� Orientation$ 
Horizontal=�68   .+�5)      �� $��
原数量:��6   $��Center=�6(   6E   �� 5*      -   5        $��txtb_OriginFCAmount��5*      $	��120��65   	 �� 
IsReadOnly	 . $��Center=�6S   
�������6�   5+      �� 5,      $��0 0 10 0q�6   $��1��6+   $��1��6<   $ 
Horizontal=�6I   .+�5-      �� $��
新数量:��6   $��Center=�6(   6E   %  Xceed.Wpf.Toolkit.IntegerUpDown  5.      -   5        $��iud_NewFCAmount��5.   j   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
   Minimum$
 0X�6'   �  �Xceed.Wpf.Toolkit.NumericUpDown`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   	Increment$	 100X�63   $	��120��6C   $��Center=�6O   6�   5/      �� 50      -	   5        $��btn_CancelMofidy��50   "   $��取消��6   $ Release=�6Y   $��0��6m   $��3��6}   $��Center=�6�   $��25��6�   $	��120��6�   6�   �� 51      -
   5        $��btn_Save��51   x   $��3��6   $��1��6"   $��Center=�63   $��25��6P   $	��120��6\   $��保存��6k   $ Release=�6�   6�   52      53      54      