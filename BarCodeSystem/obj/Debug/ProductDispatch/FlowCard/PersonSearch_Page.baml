   M S B A M L   `   `   `  ����  BarCodeSystem>    8BarCodeSystem.ProductDispatch.FlowCard.PersonSearch_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5      -   5          N� Title$  PersonSearch_Page��5
      .��5      � .+�5   
   �  � RowDefinitions 5      �� 5      $��30 �6    6.   �� 5      6#   �� 5      $��30 �6    6.   .+�5      �� .��5      �� 
�������选择人员信息67   ��5      �� 5      ��5      �  � Offset$ 1Q�6+    � Color$ 	Chocolate��66   6J   � 5      $	 0.5Q�6,   $ LightSalmon��69   6O   � 5      $ Crimson��6+   6=   5      5      &   System.Windows.Controls.DataGrid  5       -   5        $��datagrid_PersonInfo��5    h      ClipboardCopyMode$
 None=�6      
IsReadOnly .    AutoGenerateColumns .  $��1��6�      RowHeaderWidth$ 30Q�6�      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6�   ��6�   �� 	   Columns	 5!      0  *System.Windows.Controls.DataGridTextColumn  5"      ,  &System.Windows.Controls.DataGridColumn
   Header$
 人员编号��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   ��  �� Path$
 code�6W     5#      $
 人员姓名��6)    67   �� $
 name�6W   5%      � 5&      $��2��6    � ColumnDefinitions 5'      �� 5(      6)   �� 5)      6)   �� 5*      6)   .+�5,      �� -   5        $��btn_Refresh��5,      $��2��6,   $��	10 0 10 0q�6V   $��刷新列表��6i   6z   �� 5-      -   5        $��
btn_Submit��5-      $��2��6+   $��	10 0 10 0q�6T   $��选定人员��6g   6x   �� 5.       �� Orientation$ 
Horizontal=�6   $��Center=�68   $��30��6S   .+�5/      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  -   5        $��txtb_PersonInfo��5/   O      	Watermark$ 输入搜索信息��6.   $	��0 5q�6A   
���������6f   �� $ ActualWidth� �� ElementName$ 
btn_Submit��6�   �� 50      -   5        $��btn_ItemSearch��50       � Cursor$
 Hand��63      btn_SearchStyle#��[  6`   
�������
�������.��51      �� $��15��6    $��../../Images/Zoom.ico��6,   6M   52      53      54      55      56      57      