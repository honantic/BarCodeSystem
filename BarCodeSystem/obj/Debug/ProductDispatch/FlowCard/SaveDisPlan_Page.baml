   M S B A M L   `   `   `  ����  BarCodeSystem=    7BarCodeSystem.ProductDispatch.FlowCard.SaveDisPlan_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5      -   5          N� Title$  SaveDisPlan_Page��5
      .��5      � .+�5   
   �  � RowDefinitions 5      �� 5      $��30 �6    6.   �� 5      6#   �� 5      $��30 �6    6.   .+�5      �� .��5      �� -   5        $��textb_Header��5      
�������6E   ��5      �� 5      ��5      �  � Offset$ 1Q�6+    � Color$ 	Chocolate��66   6J   � 5      $	 0.5Q�6,   $ LightSalmon��69   6O   � 5      $ Crimson��6+   6=   5      5      � 5      $��1��6    � ColumnDefinitions 5       �� 5!      6)   �� 5"      $��15 �6'   64   �� 5#      6)   .+�5%      �� $��1��6   $��5��6/   
����ح�$��Stretch=�6P   $��Center=�6l   6�   &   System.Windows.Controls.DataGrid  5&      -   5        $��datagrid_DisPlanVer��5&         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 66      CanUserAddRows .     
IsReadOnly .    AutoGenerateColumns .  	   Columns	 5'      0  *System.Windows.Controls.DataGridTextColumn  5(      ,  &System.Windows.Controls.DataGridColumn
   Header$
 	料品名��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   ��  �� Path$ DPV_ItemName�6b     5)      $
 编号��6-    69   �� $ DPV_ItemCode�6a     5*      $
 版本��6-    69   �� $ DPV_VersionName�6d   5,        5-      -   5        $��datagrid_DisPlanInfo��5-   4      ClipboardCopyMode$
 None=�6    .  .  $��2��5.   3      RowHeaderWidth$ 30Q�6C   $
 Auto 6W   ��6   �� 	 50        51      $
 	工序号��6-    6:   �� $ DP_ProcessSequence�6h     52      $
 	工序名��6-    6:   �� $ DP_ProcessName�6d     53      $
 人员��6-    69   �� $ DP_PersonName�6b   55      56      � 57      $��2��6    58      �� 59      6)   �� 5:      6)   �� 5;      6)   .+�5=      �� -   5        $��btn_Refresh��5=      $��1��6,   $��	10 0 10 0q�6V   $��Center=�6i   $��Center=�6�   $��刷新列表��6�   6�   �� 5>      -   5        $��btn_SelectDisPlan��5>      $��2��62   $��	10 0 10 0q�6b   $��Center=�6u   $��Center=�6�   $��选择派工方案��6�   6�   �� 5?      -   5        $��btn_SaveDisPlan��5?      $��2��60   $��	10 0 10 0q�6^   $��Center=�6q   $��Center=�6�   $��保存派工方案��6�   6�   �� 5@      -   5        $��panel_Search��5@       �� Orientation$ 
Horizontal=�63   $��Center=�6L   $��30��6g   .+�5A      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  -	   5        $��txtb_DisPlanInfo��5A   O      	Watermark$ 输入搜索信息��6.   $	��0 5q�6A   $��80��6g   
�������6�   �� 5B      -
   5        $��btn_DisPlanSearch��5B       � Cursor$
 Hand��66      btn_SearchStyle#��[  6d   
�������
�������.��5C      �� $��15��6    $��../../Images/Zoom.ico��6,   6M   5D      5E      5F      5G      5H      5I      