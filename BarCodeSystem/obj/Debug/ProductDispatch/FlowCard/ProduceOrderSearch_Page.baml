   M S B A M L   `   `   `  ����  BarCodeSystemD    >BarCodeSystem.ProductDispatch.FlowCard.ProduceOrderSearch_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5      -   5          N� Title$  生产订单筛选页面��5
      .��5      � .+�5   
   �  � RowDefinitions 5      �� 5      $��30 �6    6.   �� 5      6#   �� 5      $��30 �6    6.   .+�5      �� .��5      �� 
�������选择生产订单67   ��5      �� 5      ��5      �  � Offset$ 1Q�6+    � Color$ 	Chocolate��66   6J   � 5      $	 0.5Q�6,   $ LightSalmon��69   6O   � 5      $ Crimson��6+   6=   5      5      &   System.Windows.Controls.DataGrid  5       -   5        $��datagrid_ProduceOrderInfo��5    n      ClipboardCopyMode$
 None=�6      
IsReadOnly .    AutoGenerateColumns .  $��1��6�      RowHeaderWidth$ 30Q�6�      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6�   ��6�   �� 	   Columns	 5!      0  *System.Windows.Controls.DataGridTextColumn  5"      ,  &System.Windows.Controls.DataGridColumn
   Header$
 订单编号��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   ��  �� Path$ PO_Code�6Z     5#      $
 料品编号��6)    67   �� $ PO_ItemCode�6^     5$      $
 料品名称��6)    67   �� $ PO_ItemName�6^     5%      $
 料品型号��6)    67   �� $ PO_ItemVersion�6a     5&      $
 料品规格��6)    67   �� $ PO_ItemSpec�6^     5'      $
 订单数量��6)    67   �� $ PO_OrderAmount�6a     5(      $
 已开工数量��6)    68   �� $ PO_StartAmount�6b   5*      � 5+      $��2��6    � ColumnDefinitions 5,      �� 5-      6)   �� 5.      6)   �� 5/      6)   .+�51      �� -   5        $��
btn_Submit��51      $��2��6+   $��	10 0 10 0q�6T   $��选定生产订单��6g   6z   �� 52      -   5        $��btn_Refresh��52      $��1��6,   $��	10 0 10 0q�6V   $��刷新列表��6i   6z   �� 53       �� Orientation$ 
Horizontal=�6   $��Center=�68   $��30��6S   .+�54      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  -   5        $��txtb_ProduceOrderInfo��54   P      	Watermark$ 请输入搜索信息��6.   $	��0 5q�6B   
���������6m   �� $ ActualWidth� �� ElementName$ 
btn_Submit��6�   �� 55      -   5        $��btn_ProduceOrderSearch��55       � Cursor$
 Hand��6;      btn_SearchStyle#��[  6n   
�������
�������.��56      �� $��15��6    $��../../Images/Zoom.ico��6,   6M   57      58      59      5:      5;      5<      