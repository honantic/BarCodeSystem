   M S B A M L   `   `   `  ����  BarCodeSystem0    *BarCodeSystem.StaffSalariesDetailView_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5      -   5          N� Title$$  员工工资详细信息页面��5	      .��5      � .+�5   
   �  � RowDefinitions 5      �� 5      $��30 �6    6.   �� 5      6#   �� 5      $��30 �6    6.   .+�5      �� .��5      �� 
�������员工工资详细信息69   ��5      �� 5      ��5      �  � Offset$ 1Q�6+    � Color$ 	Chocolate��66   6J   � 5      $	 0.5Q�6,   $ LightSalmon��69   6O   � 5      $ Crimson��6+   6=   5      5      &   System.Windows.Controls.DataGrid  5      -   5        $��datagrid_SalariesInfo��5   s      ClipboardCopyMode$ IncludeHeader=�6      
IsReadOnly .    AutoGenerateColumns .  $��1��5    K      RowHeaderWidth$ 30Q�6X      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6l   ��63   �� 	   Columns	 5!      0  *System.Windows.Controls.DataGridTextColumn  5"      ,  &System.Windows.Controls.DataGridColumn
   Header$
 员工编码��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   ��  �� Path$ 
PersonCode�6^     5#      $
 员工姓名��6)    67   �� $ 
PersonName�6]     5$      $
 流转卡号��6)    67   �� $ FC_Code�6Z     5%      $
 工序名称��6)    67   �� $ ProcessName�6^     5&      $
 报工时间��6)    67   �� $ 
ReportTime�6]     5'      $
 完工数量��6)    67   �� $ QulifiedAmount�6a     5(      $
 计件人数��6)    67   �� $ PeopleAmount�6_     5)      $
 工时��6)    65   �� $ WorkHour�6Y     5*      $
 计件工资(元)��6)    6:   �� $ Salary�6\   5,      � 5-      $��2��6    � ColumnDefinitions 5.      �� 5/      6)   �� 50      6)   �� 51      6)   .+�54      �� -   5        $��tbk_tip��54   c   $��0��6   $��Center=�6-   $
��Left=�6H   6t   �� 56      -   5        $��
btn_Export��56   �   $��2��6   $	��120��6)   $��80��68   $��导出��6F   $��Center=�6S   $��Right=�6n   6�   5@      5A      5B      5C      