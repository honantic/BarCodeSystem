   M S B A M L   `   `   `  ����  BarCodeSystemB    <BarCodeSystem.FileQuery.FlowCardQuery.ModifyWaySelect_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   -   5          � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ ModifyWaySelect_Window��5
   	   $	��400��6(   $	��400��65    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6A   .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      $	��300 �6   6+   �� 5      $��40 �6   6*    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   .+�5   
   �� $��请选择修改方式��6   $��3��6#   $��Bottom=�67   $��Center=�6R   6q   �� 5   
   
����ح�$��4��6)   $��3��6:   $��10q�6N   6e   �� 5   
   -   5        $��btn_SelectByHand��5   \   $��1��6   $��10 0 0 0q�6   $	��120��60   $��25��6?   $��手动选择->��6K    �� 	ClickMode$ Release=�6�   6�   �� 5   
   -   5        $��btn_SelectByScanning��5   l   $��1��6   $��0 0 10 0q�6   $��2��60   $	��120��6@   $��25��6O   $��扫描选择->��6[   $ Release=�6�   6�   �� 5   
   -   5        $��
btn_Delete��5      $��删除��6<   $��1��6I   $
��10 0q�6Y   $��3��6g   $��25��6t   $	��Top=�6�   6�   �� 5   
   -   5        $��
btn_Cancel��5   o   $��3��6   $��10  0q�6   $	��Top=�6.   $��25��6G   $	��120��6S   $��取消��6b   $ Release=�6�   6�   �� 5   
   -   5        $��btn_Save��5   }   $��3��6   $
��10 0q�6   $	��Top=�6,   $��3��6E   $��25��6U   $	��120��6a   $��保存��6p   $ Release=�6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��datagrid_AmountInfo��5   X      CanUserAddRows .  $��3��6+   $��2��6?   $��10q�6L   	   RowHeaderWidth$	 30Q�6�   
   AutoGenerateColumns
 .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6�   ��6s   ��    Columns 5       0  *System.Windows.Controls.DataGridTextColumn  5!      ,  &System.Windows.Controls.DataGridColumn   Header$ 编号��6%      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6C   ��  �� Path$ QI_Code� �� Mode$ TwoWay=�5"        5#      $ 质量问题原因名称��6%    67   �� $ QI_Name�6Z   4  .System.Windows.Controls.DataGridTemplateColumn  5$      $ 数量��6)      CellTemplate 5%      �� 5&      .R�5'      �� ��6(   �� $ FCQ_ScrapAmount�$ TwoWay=�6^   5(         CellEditingTemplate 5*      �� 5+      .R�5,      %  Xceed.Wpf.Toolkit.IntegerUpDown  $��integer_Amount��63   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 0X�6J      DefaultValue$ 0X�6V      Value 6i   �� $ FCQ_ScrapAmount�$ TwoWay=�6�   5-      5/      51      52      53      