   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulle7clr-namespace:BarCodeSystem.PublicClass.ValueConverters)BarCodeSystem.PublicClass.ValueConverters  5      A    ;BarCodeSystem.ProductDispatch.FlowCard.TechRouteSearch_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Ilocal7clr-namespace:BarCodeSystem.PublicClass.ValueConverters     5      -   5          N� Title$  工艺路线选择页面��5
      .��5      �  � 	Resources 5   
   D   >BarCodeSystem.PublicClass.ValueConverters.TrueOrFalseConverter   boolConvert��%   &	          5      6?   5       � ColumnDefinitions 5   
   �� 5      6!   �� 5      $��15 �6   6,   �� 5      $��2* �6   6,    � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      6   �� 5      $��30 �6   6*   .+�5   
   �� $��3��6   .��5      �� -   5        $��textb_PageTitle��5      $��15(�6/   
������� 工艺路线版本选择页面6\   ��5      �� 5      ��5      �  � Offset$ 1Q�6'    � Color$ 	Chocolate��62   6F   � 5       $	 0.5Q�6(   $ LightSalmon��65   6K   � 5!      $ Crimson��6'   69   5#      5%      �� 5&   
   $��5��6   $��1��6!   $��1��6/   $��Stretch=�6?   $��Stretch=�6[   
�������6�   �� 5'   
   $��1��6   ��5(      �� 5)        textb_NormalStyle#��[ 6   选择工艺版本6Q   .��5+      &   System.Windows.Controls.DataGrid  -   5        $��datagrid_RouteVersion��5+   j      ClipboardCopyMode$
 None=�6Q      
IsReadOnly .    CanUserAddRows .  	   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
	 Auto 5,      
   RowHeaderWidth$
 30Q�6*      AutoGenerateColumns .     Columns 5.      0  *System.Windows.Controls.DataGridTextColumn  5/      ,  &System.Windows.Controls.DataGridColumn   Header$ 编号��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   ��  �� Path$ TRV_VersionCode�6`     50      $ 名称��6)    65   �� $ TRV_VersionName�6`     51      $ 是否默认��6)    67   �� $ TRV_IsDefaultVer� �� 	Converter# [  6�   53      54      �� 56   
   $��1��6   $��3��6    ��57      �� 58      #��[ 6   工序信息6O   .��5:        -   5        $��datagrid_RouteProcess��5:   0   $
 None=�6    .   . $
 30Q�6v    .   5;        5<      $ 行号��6)    65   �� $ TR_ProcessSequence�6c     5=      $ 工序名称��6)    67   �� $ TR_ProcessName�6a   5?      5@      � 5A   
   $��2��6   $��3��6    5B      �� 5C      6%   �� 5D      6%   �� 5E      6%   .+�5G      �� -   5        $��label_ErrorInfo��5G      
������$y�Hidden=�6C   .��6W   ;9没有工艺信息，请先在系统中录入工艺信息6l   �� 5H      -   5        $��
btn_Submit��5H   E   $��2��6   $��选定工艺路线��6%   $
��10 0q�67   6r   �� 5I      -   5        $��btn_Refresh��5I   C   $��1��6   $��刷新列表��6%   $
��10 0q�65   6r   5J      5K      5L      